using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Stubborn.Tests
{
    [YamlIgnore]
    class DummyIgnoredItem
    {
    }

    [YamlFormat(AlwaysNested = true)]
    class DummyAlwaysNestedList : List<object>
    {

    }

    [TestClass]
    public class Sequences
    {
        [TestMethod]
        public void TestSequences()
        {
            Assert.AreEqual(
                "",
                YamlSerializer.Serialize(
                    new string[] { }
                ));

            Assert.AreEqual(
                "test\n",
                YamlSerializer.Serialize(
                    new string[] { "test" }
                ));

            Assert.AreEqual(
                "- first\n" +
                "- second\n",
                YamlSerializer.Serialize(
                    new List<string> { "first", "second" }
                ));
        }

        [TestMethod]
        public void TestIgnoredItems()
        {
            Assert.AreEqual(
                "test\n",
                YamlSerializer.Serialize(
                    new object[] { null, "test" }
                ));

            Assert.AreEqual(
                "test\n",
                YamlSerializer.Serialize(
                    new object[] { "test", null, null }
                ));

            Assert.AreEqual(
                "- third\n" +
                "- fifth\n",
                YamlSerializer.Serialize(
                    new object[] { null, null, "third", new DummyIgnoredItem(), "fifth", null }
                ));
        }

        [TestMethod]
        public void TestNestedSequences()
        {
            Assert.AreEqual(
                "- a\n" +
                "- - b1\n" +
                "  - b2a\n" +
                "  - b3\n" +
                "- c\n",
                YamlSerializer.Serialize(
                    new List<object>
                    {
                        "a",
                        new List<object>
                        {
                            "b1",
                            new List<object>
                            {
                                "b2a"
                            },
                            "b3"
                        },
                        "c"
                    }));

            Assert.AreEqual(
                "- a\n" +
                "-\n" +
                "  - b1\n" +
                "  -\n" +
                "    - b2a\n" +
                "  - b3\n" +
                "- c\n",
                YamlSerializer.Serialize(
                    new List<object>
                    {
                        "a",
                        new DummyAlwaysNestedList
                        {
                            "b1",
                            new DummyAlwaysNestedList
                            {
                                "b2a"
                            },
                            "b3"
                        },
                        "c"
                    }));
        }

        [TestMethod]
        public void TestMaxDepth()
        {
            Assert.AreEqual(
                "- - 1\n",
                YamlSerializer.Serialize(
                    new List<object>()
                    {
                        new List<object>()
                        {
                            new List<object>()
                            {
                                "1"
                            }
                        }
                    },
                    new YamlSerializationOptions
                    {
                        MaxDepth = 3
                    }));

            Assert.ThrowsException<YamlSerializationTooDeep>(() =>
                YamlSerializer.Serialize(
                    new List<object>()
                    {
                        new List<object>()
                        {
                            new List<object>()
                            {
                                new List<object>()
                            }
                        }
                    },
                    new YamlSerializationOptions
                    {
                        MaxDepth = 3
                    }));
        }
    }
}
