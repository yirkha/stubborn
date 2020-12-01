using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Stubborn
{
    public class YamlSerializer
    {
        /// <summary>
        /// Serialize object into YAML
        /// </summary>
        /// <remarks>
        /// <c>obj</c> will be recursively traversed and all public properties
        /// will be serialized to the YAML format. The exact formatting and
        /// appearance choices can be set through the global options here or
        /// altered on a property/item basis through property/class attributes.
        /// </remarks>
        /// <param name="obj">Object to serialize</param>
        /// <param name="options">Global options</param>
        /// <returns>String with the YAML representation of <c>obj</c></returns>
        public static string Serialize(object obj, YamlSerializationOptions options = null)
        {
            var s = new YamlSerializer(options);
            if (obj != null)
            {
                s.SerializeItem(obj);
            }
            return s.Finish();
        }

        private YamlSerializationOptions _globals;
        private CombinedOptions _options;
        private YamlWriter _writer;
        private int _depth;
        private AllowInline _allowInline;

        enum AllowInline { Nothing, Anything, PlainValues };

        private YamlSerializer(YamlSerializationOptions options = null)
        {
            _globals = options ?? new YamlSerializationOptions();
            _writer = new YamlWriter();
            _depth = 0;
            _allowInline = AllowInline.Nothing;

            Start();
        }

        private void Start()
        {
            if (_globals.DocumentBoundaries)
            {
                _writer.AppendDocumentStart();
            }

            if (!_globals.DisableOverrides)
            {
                if (_globals.PrologueComment != null)
                {
                    _writer.AppendComment(_globals.PrologueCommentStyle, _globals.PrologueComment, _globals.Width);
                }
                _writer.AppendBlankLinesAfter(_globals.PrologueMargin ?? ((_globals.PrologueComment != null) ? 1 : 0));
            }
        }

        private string Finish()
        {
            if (!_globals.DisableOverrides)
            {
                _writer.AppendBlankLinesBefore(_globals.EpilogueMargin ?? ((_globals.EpilogueComment != null) ? 1 : 0));
                if (_globals.EpilogueComment != null)
                {
                    _writer.AppendComment(_globals.EpilogueCommentStyle, _globals.EpilogueComment, _globals.Width);
                }
            }

            if (_globals.DocumentBoundaries)
            {
                _writer.AppendDocumentEnd();
            }

            return _writer.ToString();
        }

        private bool IsIgnored(object item, IEnumerable<YamlIgnoreAttribute> attrs)
        {
            string str = null;

            foreach (var attr in attrs)
            {
                if (attr.IfEquals == null && attr.IfEqualsStr == null && !attr.IfEmpty)
                {
                    return true;
                }

                if (attr.IfEquals != null && attr.IfEquals.Equals(item))
                {
                    return true;
                }

                if (attr.IfEqualsStr != null)
                {
                    if (str == null)
                    {
                        str = Stringify(item);
                    }
                    if (attr.IfEqualsStr == str)
                    {
                        return true;
                    }
                }

                if (attr.IfEmpty && (item is IEnumerable e) && !e.GetEnumerator().MoveNext())
                {
                    return true;
                }
            }

            return false;
        }

        private void SerializeItem(object item, bool list = false, string key = null,
            IEnumerable<Attribute> extraAttrs = null, object commentSource = null)
        {
            _options = new CombinedOptions(_globals)
                .OverrideWith(CustomAttributeExtensions.GetCustomAttributes(item.GetType(), true))
                .OverrideWith(extraAttrs);

            if (_options.Ignores.Count > 0 && IsIgnored(item, _options.Ignores))
            {
                return;
            }

            if ((_allowInline == AllowInline.Anything || (_allowInline == AllowInline.PlainValues && !list && key == null)) &&
                _options.BlankLinesBefore == 0 && !_options.Comments.Any(attr => IsCommentBefore(attr)))
            {
                // Do not start a new line
            }
            else
            {
                _writer.AppendBlankLinesBefore(_options.BlankLinesBefore);
                AppendComments(commentSource ?? item, before: true);
                _writer.StartNewLine();
            }

            if (list)
            {
                _writer.AppendSequenceItemPrefix();
                _allowInline = _options.AlwaysNested ? AllowInline.Nothing : AllowInline.Anything;
            }
            else if (key != null)
            {
                SerializeString(_options.Name ?? key, true);
                _writer.AppendKeyValueSeparator();
                _allowInline = AllowInline.PlainValues;
            }

            if (item is string)
            {
                SerializeString((string)item, false);
            }
            else if (item is bool)
            {
                SerializeBool((bool)item);
            }
            else if (item.GetType().IsPrimitive)
            {
                SerializeString(item.ToString(), false);
            }
            else
            {
                if (++_depth > _globals.MaxDepth)
                {
                    throw new YamlSerializationTooDeep();
                }

                bool nextIsList = (item is IEnumerable) && !(item is IDictionary);
                bool needsIndent = (list || ((key != null) && !nextIsList));
                if (needsIndent)
                {
                    _writer.Indent(_options.IndentStep);
                }
                var prevOptions = _options;

                if (item is IDictionary)
                {
                    SerializeDictionary((IDictionary)item);
                }
                else if (item is IEnumerable)
                {
                    SerializeList((IEnumerable)item);
                }
                else
                {
                    SerializeObject(item);
                }

                _options = prevOptions;
                if (needsIndent)
                {
                    _writer.Undent();
                }
                --_depth;
            }

            AppendComments(commentSource ?? item, after: true);
            _writer.AppendBlankLinesAfter(_options.BlankLinesAfter);

            _allowInline = AllowInline.Nothing;
        }

        private void SerializeDictionary(IDictionary dict)
        {
            foreach (DictionaryEntry item in dict)
            {
                if (item.Value == null)
                {
                    continue;
                }

                SerializeItem(item.Value, key: item.Key.ToString());
            }
        }

        private void SerializeList(IEnumerable list)
        {
            var enumerator = list.GetEnumerator();

            // Handle empty lists, skip any leading nulls
            do
            {
                if (!enumerator.MoveNext())
                {
                    return;
                }
            }
            while (enumerator.Current == null);

            // Display as a simple value if there is only 1 actual item
            var first = enumerator.Current;
            bool hasMore = enumerator.MoveNext();
            while (hasMore && enumerator.Current == null)
            {
                hasMore = enumerator.MoveNext();
            }
            if (!hasMore && !_options.AlwaysNested && IsPlainValue(first))
            {
                SerializeItem(first);
                return;
            }
            SerializeItem(first, list: true);

            // Process the rest of the list
            while (hasMore)
            {
                if (enumerator.Current != null)
                {
                    SerializeItem(enumerator.Current, list: true);
                }
                hasMore = enumerator.MoveNext();
            }
        }

        private void SerializeObject(object obj)
        {
            var properties = obj.GetType().GetProperties();
            if (properties.Length == 0 || _options.Format != null)
            {
                SerializeString(Stringify(obj), false);
                return;
            }

            IEnumerable<PropertyInfo> sorted;
            if (!_options.DisableOverrides)
            {
                var prev = -1;
                var stabilizer = 0;
                sorted = properties
                    .Select(p =>
                    {
                        var attr = p.GetCustomAttribute<YamlOrderAttribute>();
                        var current = (attr != null) ? attr.Order : (prev + 1);
                        prev = (attr != null && !attr.ThisOnly) ? current : (prev + 1);
                        return (Pos: current * 1000 + stabilizer++, Prop: p);
                    })
                    .OrderBy(p => p.Pos)
                    .Select(p => p.Prop);
            }
            else
            {
                sorted = properties;
            }

            foreach (var prop in sorted)
            {
                var value = prop.GetValue(obj);
                if (value == null)
                {
                    continue;
                }

                SerializeItem(
                    value,
                    key: FormatPropertyName(prop.Name),
                    extraAttrs: prop.GetCustomAttributes(),
                    commentSource: obj);
            }
        }

        private string Stringify(object obj)
        {
            if (_options.Format != null && obj is IFormattable)
            {
                return ((IFormattable)obj).ToString(_options.Format, CultureInfo.InvariantCulture);
            }
            else
            {
                return obj.ToString();
            }
        }

        private string FormatPropertyName(string name)
        {
            string formatted = name;

            switch (_globals.PropertyNamingStyle)
            {
                case YamlNamingStyle.UpperCamelCase:
                    formatted = Regex.Replace(formatted, @"(?:^_*|_+)(.)", m => m.Groups[1].Value.ToUpperInvariant());
                    break;

                case YamlNamingStyle.LowerCamelCase:
                    formatted = Regex.Replace(formatted, @"_+(.)", m => m.Groups[1].Value.ToUpperInvariant());
                    formatted = Regex.Replace(formatted, @"^(.)", m => m.Groups[1].Value.ToLowerInvariant());
                    break;
            }

            return formatted;
        }

        private static bool IsPlainValue(object item)
            => (item is string) || item.GetType().IsPrimitive;

        private static bool IsCommentBefore(YamlCommentAttribute attr)
            => (attr.Style == YamlCommentStyle.SmallAbove ||
                attr.Style == YamlCommentStyle.MediumAbove ||
                attr.Style == YamlCommentStyle.LargeAbove);

        private void AppendComments(object obj, bool before = false, bool after = false)
        {
            foreach (var attr in _options.Comments)
            {
                if ((!before && IsCommentBefore(attr)) || (!after && !IsCommentBefore(attr)))
                {
                    continue;
                }

                if (!attr.TryGetText(obj, out var text))
                {
                    continue;
                }

                _writer.AppendComment(attr.Style, text, _globals.Width);
            }
        }

        private void SerializeString(string str, bool isKey)
        {
            if (string.IsNullOrEmpty(str) && !isKey && !_options.Quoted && !_options.Block)
            {
                return;
            }

            // These options should affect values, not the keys
            bool forceQuoted = _options.Quoted && !isKey;
            bool forceBlock = _options.Block && !isKey;

            if (forceQuoted || Regex.IsMatch(str, @"[\x00-\x09\x0B-\x1F]| $", RegexOptions.Multiline))
            {
                // The string contains special characters or trailing whitespace
                _writer.Indent(_options.IndentStep);
                _writer.AppendDoubleQuotedString(str, !isKey, _options.Block);
                _writer.Undent();
                return;
            }

            bool multiline = (str.IndexOf('\n') >= 0);
            if (!forceBlock && !multiline &&
                Regex.IsMatch(str, @"^([^ \-?:,\[\]{}#&*!|>'""%@`]|[?:-](?=[^ ,\[\]{}]))([^:#]|(?<! )#|:(?! ))*$"))
            {
                // The string is not forced to be a nested block, it is not
                // multi-line and does not contain any special characters
                _writer.AppendLiteral(str);
                return;
            }

            if (forceBlock || (multiline && !isKey))
            {
                _writer.Indent(_options.IndentStep);
                _writer.AppendLiteralStringBlock(str);
                _writer.Undent();
            }
            else
            {
                _writer.AppendDoubleQuotedString(str, false, false);
            }
        }

        private void SerializeBool(bool value)
        {
            _writer.AppendLiteral(value ? "true" : "false");
        }
    }

    public class YamlSerializationTooDeep : Exception
    {
        public YamlSerializationTooDeep()
            : base("YAML serialization nested too deep")
        {
        }
    }
}
