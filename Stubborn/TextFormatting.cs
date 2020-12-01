using System;
using System.Collections.Generic;

namespace Stubborn
{
    public static class TextFormatting
    {
        public static IEnumerable<string> WordWrap(string text, int width)
        {
            width = Math.Max(width, 1);

            var lineStart = 0;
            while (lineStart < text.Length)
            {
                // Find natural end of line
                var lineEnd = text.IndexOf('\n', lineStart);
                if (lineEnd < 0)
                {
                    lineEnd = text.Length;
                }

                var nextLineStart = lineEnd + 1;
                if (lineEnd - lineStart > width)
                {
                    // The line needs to be broken - find the beginning of the last word
                    nextLineStart = lineStart + width;
                    while (nextLineStart > lineStart && text[nextLineStart] != ' ')
                    {
                        nextLineStart--;
                    }

                    if (nextLineStart == lineStart)
                    {
                        // There is a word which is longer than the output width
                        while (nextLineStart < text.Length && text[nextLineStart] != ' ')
                        {
                            nextLineStart++;
                        }
                    }

                    lineEnd = nextLineStart;

                    // Avoid leading whitespace on broken lines
                    while (nextLineStart < text.Length && text[nextLineStart] == ' ')
                    {
                        nextLineStart++;
                    }
                }

                // Avoid trailing whitespace everywhere
                while (lineEnd > lineStart && text[lineEnd - 1] == ' ')
                {
                    lineEnd--;
                }

                yield return text.Substring(lineStart, lineEnd - lineStart);

                lineStart = nextLineStart;
            }

            // Return something even for empty input
            if (lineStart == 0)
            {
                yield return "";
            }
        }
    }
}
