using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;

namespace Stubborn
{
    class YamlWriter
    {
        private StringBuilder _output;
        private List<int> _indents;
        private int _lastBottomMargin;

        public YamlWriter()
        {
            _output = new StringBuilder();
            _indents = new List<int>() { 0 };
            _lastBottomMargin = 0;
        }

        public override string ToString()
        {
            // Ensure non-empty output ends with a newline
            if (_output.Length > 0 && _output[_output.Length - 1] != '\n')
            {
                _output.Append('\n');
            }

            return _output.ToString();
        }

        private int Indentation { get => _indents[_indents.Count - 1]; }

        public void Indent(int amount)
        {
            _indents.Add(Indentation + amount);
        }

        public void Undent()
        {
            _indents.RemoveAt(_indents.Count - 1);
        }

        public void StartNewLine(bool indent = true)
        {
            if ((_lastBottomMargin > 0) || (_output.Length > 0 && _output[_output.Length - 1] != '\n'))
            {
                // Avoid trailing whitespace
                if (_output[_output.Length - 1] == ' ')
                {
                    _output[_output.Length - 1] = '\n';
                }
                else
                {
                    _output.Append('\n');
                }
            }

            _lastBottomMargin = 0;

            if (indent)
            {
                _output.Append(' ', Indentation);
            }
        }

        public void AppendBlankLinesBefore(int topMargin)
        {
            int blankLinesBefore = Math.Max(topMargin - _lastBottomMargin, 0);
            StartNewLine(false);
            _output.Append('\n', blankLinesBefore);
        }

        public void AppendBlankLinesAfter(int bottomMargin)
        {
            _lastBottomMargin = bottomMargin;
            _output.Append('\n', bottomMargin);
        }

        public void AppendComment(YamlCommentStyle style, string text, int rightMargin)
        {
            switch (style)
            {
                case YamlCommentStyle.SameLine:
                    _output.Append("  # ");
                    _output.Append(text);
                    break;

                case YamlCommentStyle.SmallAbove:
                    foreach (var line in TextFormatting.WordWrap(text, rightMargin - Indentation - 2))
                    {
                        StartNewLine();
                        _output.Append("# ");
                        _output.Append(line);
                    }
                    break;

                case YamlCommentStyle.MediumAbove:
                    StartNewLine();
                    _output.Append('#');
                    foreach (var line in TextFormatting.WordWrap(text, rightMargin - Indentation - 2))
                    {
                        StartNewLine();
                        _output.Append("# ");
                        _output.Append(line);
                    }
                    StartNewLine();
                    _output.Append('#');
                    break;

                case YamlCommentStyle.LargeAbove:
                    var markerLen = Math.Max(rightMargin - Indentation, 5);
                    StartNewLine();
                    _output.Append('#', markerLen);
                    foreach (var line in TextFormatting.WordWrap(text, rightMargin - Indentation - 2))
                    {
                        StartNewLine();
                        _output.Append("# ");
                        _output.Append(line);
                    }
                    StartNewLine();
                    _output.Append('#', markerLen);
                    break;
            }
        }

        private void AppendEscapedString(string input)
        {
            int start = 0;
            for (var i = 0; i < input.Length; i++)
            {
                var ch = (int)input[i];
                if (ch >= 32 && ch != '\\')
                {
                    continue;
                }
                _output.Append(input, start, i - start);
                start = i + 1;
                switch (ch)
                {
                    case 9:
                        _output.Append("\\t");
                        break;

                    case 10:
                        _output.Append("\\n");
                        break;

                    case 13:
                        _output.Append("\\r");
                        break;

                    case '\\':
                        _output.Append("\\\\");
                        break;

                    default:
                        _output.AppendFormat("\\x{0:X2}", ch);
                        break;
                }
            }
            _output.Append(input, start, input.Length - start);
        }

        public void AppendDoubleQuotedString(string text, bool keepLines, bool block)
        {
            _output.Append('"');
            if (!keepLines)
            {
                AppendEscapedString(text);
            }
            else
            {
                var first = true;
                foreach (var line in text.Split('\n'))
                {
                    if (first)
                    {
                        first = false;
                        if (block)
                        {
                            _output.Append('\\');
                            StartNewLine();
                        }
                    }
                    else
                    {
                        _output.Append("\\n\\");
                        StartNewLine();
                    }
                    if (line.Length > 0 && line[0] == ' ')
                    {
                        _output.Append('\\');
                    }
                    AppendEscapedString(line);
                }
            }
            _output.Append('"');
        }

        private static IEnumerable<T> SkipLast1<T>(IEnumerable<T> input)
        {
            // IEnumerable<>.SkipLast(n) is not available in .NET Standard 2.0
            using (var enumerator = input.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    yield break;
                }
                T item = enumerator.Current;
                while (enumerator.MoveNext())
                {
                    yield return item;
                    item = enumerator.Current;
                }
            }
        }

        public void AppendLiteralStringBlock(string text)
        {
            if (text == "")
            {
                _output.Append("|-");
                return;
            }

            IEnumerable<string> lines = text.Split('\n');

            _output.Append('|');
            if (!text.EndsWith("\n"))
            {
                _output.Append('-');
            }
            else
            {
                lines = SkipLast1(lines);
            }
            if (text.StartsWith(" "))
            {
                _output.Append(Indentation);
            }

            foreach (var line in lines)
            {
                _output.Append('\n');
                if (line.Length > 0)
                {
                    StartNewLine();
                    _output.Append(line);
                }
            }
        }

        public void AppendLiteral(string text)
        {
            _output.Append(text);
        }

        public void AppendSequenceItemPrefix()
        {
            _output.Append("- ");
        }

        public void AppendKeyValueSeparator()
        {
            _output.Append(": ");
        }

        public void AppendDocumentStart()
        {
            StartNewLine();
            _output.Append("---");
        }

        public void AppendDocumentEnd()
        {
            StartNewLine();
            _output.Append("...");
        }
    }
}
