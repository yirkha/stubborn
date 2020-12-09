using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Stubborn.Tests
{
    [TestClass]
    public class TextFormattingTests
    {
        [TestMethod]
        public void TestWordWrap()
        {
            CollectionAssert.AreEqual(
                new string[]
                {
                    ""
                },
                TextFormatting.WordWrap(
                    "",
                    10).ToArray());
            CollectionAssert.AreEqual(
                new string[]
                {
                    ""
                },
                TextFormatting.WordWrap(
                    " ",
                    10).ToArray());

            CollectionAssert.AreEqual(
                new string[]
                {
                    "a"
                },
                TextFormatting.WordWrap(
                    "a",
                    10).ToArray());
            CollectionAssert.AreEqual(
                new string[]
                {
                    "a"
                },
                TextFormatting.WordWrap(
                    "a ",
                    10).ToArray());

            CollectionAssert.AreEqual(
                new string[]
                {
                    "ab"
                },
                TextFormatting.WordWrap(
                    "ab",
                    10).ToArray());
            CollectionAssert.AreEqual(
                new string[]
                {
                    "ab cde fgh"
                },
                TextFormatting.WordWrap(
                    "ab cde fgh",
                    10).ToArray());

            CollectionAssert.AreEqual(
                new string[] {
                    "ab cde fgh",
                    "ijk"
                },
                TextFormatting.WordWrap(
                    "ab cde fgh ijk",
                    10).ToArray());
            CollectionAssert.AreEqual(
                new string[]
                {
                    "ab cde",
                    "fghij k"
                },
                TextFormatting.WordWrap(
                    "ab cde fghij k",
                    10).ToArray());

            CollectionAssert.AreEqual(
                new string[]
                {
                    "a bb  ccc",
                    "dddd"
                },
                TextFormatting.WordWrap(
                    "a bb  ccc   dddd",
                    10).ToArray());
            CollectionAssert.AreEqual(
                new string[]
                {
                    "a",
                    "b",
                    "c        d"
                },
                TextFormatting.WordWrap(
                    "a         b             c        d",
                    10).ToArray());

            CollectionAssert.AreEqual(
                new string[]
                {
                    "abc",
                    "defghijklmno"
                },
                TextFormatting.WordWrap(
                    "abc defghijklmno",
                    10).ToArray());

            CollectionAssert.AreEqual(
                new string[]
                {
                    "abc",
                    "defghijklmno"
                },
                TextFormatting.WordWrap(
                    "abc defghijklmno ",
                    10).ToArray());

            CollectionAssert.AreEqual(
                new string[]
                {
                    "abc",
                    "defghijklmno",
                    "pqr"
                },
                TextFormatting.WordWrap(
                    "abc defghijklmno pqr",
                    10).ToArray());

            CollectionAssert.AreEqual(
                new string[]
                {
                    "Lorem ipsum",
                    "dolor sit",
                    "amet,",
                    "consectetur",
                    "adipiscing",
                    "elit."
                },
                TextFormatting.WordWrap(
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                    12).ToArray());

            CollectionAssert.AreEqual(
                new string[]
                {
                    "  Lorem ipsum dolor sit amet,",
                    "consectetur adipiscing elit.",
                    "  Vestibulum viverra interdum",
                    "sapien, eu convallis augue",
                    "pharetra eu.",
                    "  Nam nec diam vitae quam",
                    "mattis sollicitudin.",
                    "  In eleifend nisl urna.",
                    "  Praesent tristique dignissim",
                    "consequat.",
                    "  Nunc quis felis sit amet",
                    "dolor malesuada luctus.",
                    "  Pellentesque habitant morbi",
                    "tristique senectus et netus et",
                    "malesuada fames ac turpis",
                    "egestas.",
                    "  Aliquam quis dolor quis nunc",
                    "volutpat porta.",
                    "  Donec quis rhoncus purus.",
                    "  Fusce finibus semper eros",
                    "vitae congue.",
                    "  Donec tempus congue justo ut",
                    "luctus."
                },
                TextFormatting.WordWrap(
                    "  Lorem ipsum dolor sit amet, consectetur adipiscing elit.\n" +
                    "  Vestibulum viverra interdum sapien, eu convallis augue pharetra eu.\n" +
                    "  Nam nec diam vitae quam mattis sollicitudin.\n" +
                    "  In eleifend nisl urna.\n" +
                    "  Praesent tristique dignissim consequat.\n" +
                    "  Nunc quis felis sit amet dolor malesuada luctus.\n" +
                    "  Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas.\n" +
                    "  Aliquam quis dolor quis nunc volutpat porta.\n" +
                    "  Donec quis rhoncus purus.\n" +
                    "  Fusce finibus semper eros vitae congue.\n" +
                    "  Donec tempus congue justo ut luctus.\n",
                    30).ToArray());
        }
    }
}
