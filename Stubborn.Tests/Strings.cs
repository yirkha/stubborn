using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Stubborn.Tests
{
    class DummyQuotedStrings
    {
        public string SpecialChars => "ttt\ttab";

        public string ReservedChars => "*yuck*";

        public string Quotes => " I really \"like\" this ";

        public string TrailingWhitespace => "superfluous ";

        [YamlFormat(Quoted = true)]
        public string ExplicitlyQuoted => "unnecessary";

        public string Multiline => "jingle \a\a\njingle \a\a\n  jingle all the way";

        [YamlFormat(Block = true)]
        public string ForcedBlock => "a new line ";

        [YamlFormat(Quoted = true)]
        public string Empty => "";

        [YamlFormat(DoubleQuoted = true)]
        public string DoubleEmpty => "";

        [YamlFormat(Quoted = true)]
        public string HasSingle => "console.log('Hello world')";

        [YamlFormat(Quoted = true)]
        public string HasDouble => "console.log(\"Hello world\")";

        [YamlFormat(Quoted = true)]
        public string PrefersDouble => "this.combines(\"quoting styles\").and('ends up').being(\"double 'quoted'\")";

        [YamlName(" nasty key ")]
        public string Irrelevant1 => "it's simple: everything works";

        [YamlName("even\nworse")]
        public string Irrelevant2 => "still ok";

        [YamlName("sum\rof\\all\"fears")]
        public string Irrelevant3 => "no problem";
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
                "'':\n",
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
                "reservedChars: '*yuck*'\n" +
                "quotes: ' I really \"like\" this '\n" +
                "trailingWhitespace: 'superfluous '\n" +
                "explicitlyQuoted: 'unnecessary'\n" +
                "multiline: \"jingle \\x07\\x07\\n\\\n" +
                "  jingle \\x07\\x07\\n\\\n" +
                "  \\  jingle all the way\"\n" +
                "forcedBlock: \"\\\n" +
                "  a new line \"\n" +
                "empty: ''\n" +
                "doubleEmpty: \"\"\n" +
                "hasSingle: \"console.log('Hello world')\"\n" +
                "hasDouble: 'console.log(\"Hello world\")'\n" +
                "prefersDouble: \"this.combines(\\\"quoting styles\\\").and('ends up').being(\\\"double 'quoted'\\\")\"\n" +
                "' nasty key ': \"it's simple: everything works\"\n" +
                "\"even\\nworse\": still ok\n" +
                "\"sum\\rof\\\\all\\\"fears\": no problem\n",
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
