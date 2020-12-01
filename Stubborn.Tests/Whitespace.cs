using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Stubborn.Tests
{
    class DummyVerticalWhitespace
    {
        public string One => "1";

        public string Two => "2";

        [YamlFormat(BlankLinesBefore = 1)]
        public string Ten => "10";

        public string Eleven => "11";

        [YamlFormat(BlankLinesBefore = 2, BlankLinesAfter = 3)]
        public string Twenty => "20";

        [YamlFormat(BlankLinesAfter = 2)]
        public string Forty => "40";

        [YamlFormat(BlankLinesBefore = 2)]
        [YamlFormat(BlankLinesAfter = 3)]
        public string FortyTwo => "42";

        [YamlFormat(BlankLinesBefore = 1)]
        public string Fifty => "50";
    }

    [YamlFormat(BlankLinesAfter = 1)]
    class DummyWaitStep
    {
        public override string ToString() => "wait";
    }

    [TestClass]
    public class Whitespace
    {
        [TestMethod]
        public void TestVerticalWhitespace()
        {
            Assert.AreEqual(
                "one: 1\n" +
                "two: 2\n" +
                "\n" +
                "ten: 10\n" +
                "eleven: 11\n" +
                "\n" +
                "\n" +
                "twenty: 20\n" +
                "\n" +
                "\n" +
                "\n" +
                "forty: 40\n" +
                "\n" +
                "\n" +
                "fortyTwo: 42\n" +
                "\n" +
                "\n" +
                "\n" +
                "fifty: 50\n",
                YamlSerializer.Serialize(
                    new DummyVerticalWhitespace()));
        }

        [TestMethod]
        public void TestVerticalItemWhitespace()
        {
            Assert.AreEqual(
                "- wash\n" +
                "- rinse\n" +
                "- conditioner\n" +
                "- wait\n" +
                "\n" +
                "- rinse\n" +
                "- wait\n" +
                "\n" +
                "- dry\n",
                YamlSerializer.Serialize(
                    new object[]
                    {
                        "wash", "rinse", "conditioner", new DummyWaitStep(),
                        "rinse", new DummyWaitStep(),
                        "dry"
                    }));
        }
    }
}
