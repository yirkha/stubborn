using System;
using System.Collections.Generic;
using System.Linq;

namespace Stubborn
{
    class CombinedOptions
    {
        public bool DisableOverrides { get; set; } = false;
        public int IndentStep { get; set; } = 2;
        public string Name { get; set; }
        public List<YamlIgnoreAttribute> Ignores { get; } = new List<YamlIgnoreAttribute>();
        public string Format { get; set; }
        public int BlankLinesBefore { get; set; } = 0;
        public int BlankLinesAfter { get; set; } = 0;
        public bool AlwaysNested { get; set; } = false;
        public bool Quoted { get; set; } = false;
        public bool DoubleQuoted { get; set; } = false;
        public bool Block { get; set; } = false;
        public List<YamlCommentAttribute> Comments { get; } = new List<YamlCommentAttribute>();

        public CombinedOptions(YamlSerializationOptions options)
        {
            DisableOverrides = options.DisableOverrides;
            IndentStep = options.IndentStep;
        }

        public CombinedOptions OverrideWith(IEnumerable<Attribute> attributes)
        {
            if (attributes == null)
            {
                return this;
            }

            if (DisableOverrides)
            {
                Ignores.AddRange(attributes
                    .Where(attr => attr is YamlIgnoreAttribute)
                    .Select(attr => (YamlIgnoreAttribute)attr)
                    .Where(attr => (attr.IfEquals == null) && (attr.IfEquals == null)));
                return this;
            }

            foreach (var attr in attributes.Reverse())
            {
                var type = attr.GetType();
                if (type == typeof(YamlNameAttribute))
                {
                    Name = ((YamlNameAttribute)attr).Name;
                }
                else if (type == typeof(YamlIgnoreAttribute))
                {
                    Ignores.Add((YamlIgnoreAttribute)attr);
                }
                else if (type == typeof(YamlFormatAttribute))
                {
                    var fmt = (YamlFormatAttribute)attr;
                    if (fmt.Format != null)
                    {
                        Format = fmt.Format;
                    }
                    if (fmt.MaybeBlankLinesBefore.HasValue)
                    {
                        BlankLinesBefore = fmt.MaybeBlankLinesBefore.Value;
                    }
                    if (fmt.MaybeBlankLinesAfter.HasValue)
                    {
                        BlankLinesAfter = fmt.MaybeBlankLinesAfter.Value;
                    }
                    if (fmt.MaybeAlwaysNested.HasValue)
                    {
                        AlwaysNested = fmt.MaybeAlwaysNested.Value;
                    }
                    if (fmt.MaybeQuoted.HasValue)
                    {
                        Quoted = fmt.MaybeQuoted.Value;
                    }
                    if (fmt.MaybeDoubleQuoted.HasValue)
                    {
                        DoubleQuoted = fmt.MaybeDoubleQuoted.Value;
                    }
                    if (fmt.MaybeBlock.HasValue)
                    {
                        Block = fmt.MaybeBlock.Value;
                    }
                    if (fmt.MaybeIndentStep.HasValue)
                    {
                        IndentStep = fmt.MaybeIndentStep.Value;
                    }
                }
                else if (type == typeof(YamlCommentAttribute))
                {
                    Comments.Add((YamlCommentAttribute)attr);
                }
            }

            return this;
        }
    }
}
