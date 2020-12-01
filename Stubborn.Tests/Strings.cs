using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Stubborn.Tests
{
    class DummyQuotedStrings
    {
        public string SpecialChars => "ttt\ttab";

        public string ReservedChars => "*yuck*";

        public string TrailingWhitespace => "superfluous ";

        [YamlFormat(Quoted = true)]
        public string ExplicitlyQuoted => "unnecessary";

        public string Multiline => "jingle \a\a\njingle \a\a\n  jingle all the way";

        [YamlFormat(Block = true)]
        public string ForcedBlock => "a new line ";

        [YamlFormat(Quoted = true)]
        public string Empty => "";
    }

    class DummyBlockStrings
    {
        public string Multiline => "just\nmultiple\nlines\n";

        public string BlankLine => "first\n\nthird\n";

        public string NoFinalNewLine => "ends\nright\nhere";

        public string SomeLineWhitespace => "function hello() {\n  return \"hi\";\n}\n";

        public string FirstLineWhitespace => "  function hello() {\n    return \"hi\";\n  }\n";

        public string BothIndicators => "  hi\nthere";

        [YamlFormat(Block = true)]
        public string ForcedBlock => "unnecessary";

        [YamlFormat(Block = true)]
        public string Empty => "";
    }

    [TestClass]
    public class Strings
    {
        [TestMethod]
        public void TestEmptyStrings()
        {
            Assert.AreEqual(
                "",
                YamlSerializer.Serialize(
                    ""));

            Assert.AreEqual(
                "\"\":\n",
                YamlSerializer.Serialize(
                    new Dictionary<string, string>()
                    {
                        { "", "" }
                    }));
        }

        [TestMethod]
        public void TestPlainStrings()
        {
            Assert.AreEqual(
                "test\n",
                YamlSerializer.Serialize(
                    "test"));

            Assert.AreEqual(
                "this#is a !completely:fine{plain}string\n",
                YamlSerializer.Serialize(
                    "this#is a !completely:fine{plain}string"));
        }

        [TestMethod]
        public void TestQuotedStrings()
        {
            Assert.AreEqual(
                "specialChars: \"ttt\\ttab\"\n" +
                "reservedChars: \"*yuck*\"\n" +
                "trailingWhitespace: \"superfluous \"\n" +
                "explicitlyQuoted: \"unnecessary\"\n" +
                "multiline: \"jingle \\x07\\x07\\n\\\n" +
                "  jingle \\x07\\x07\\n\\\n" +
                "  \\  jingle all the way\"\n" +
                "forcedBlock: \"\\\n" +
                "  a new line \"\n" +
                "empty: \"\"\n",
                YamlSerializer.Serialize(
                    new DummyQuotedStrings()));
        }

        [TestMethod]
        public void TestBlockStrings()
        {
            Assert.AreEqual(
                "multiline: |\n" +
                "  just\n" +
                "  multiple\n" +
                "  lines\n" +
                "blankLine: |\n" +
                "  first\n" +
                "\n" +
                "  third\n" +
                "noFinalNewLine: |-\n" +
                "  ends\n" +
                "  right\n" +
                "  here\n" +
                "someLineWhitespace: |\n" +
                "  function hello() {\n" +
                "    return \"hi\";\n" +
                "  }\n" +
                "firstLineWhitespace: |2\n" +
                "    function hello() {\n" +
                "      return \"hi\";\n" +
                "    }\n" +
                "bothIndicators: |-2\n" +
                "    hi\n" +
                "  there\n" +
                "forcedBlock: |-\n" +
                "  unnecessary\n" +
                "empty: |-\n",
                YamlSerializer.Serialize(
                    new DummyBlockStrings()));
        }
    }
}
