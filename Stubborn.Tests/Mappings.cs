using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Stubborn.Tests
{
    [TestClass]
    public class Mappings
    {
        [TestMethod]
        public void TestMappings()
        {
            Assert.AreEqual(
                "",
                YamlSerializer.Serialize(
                    new Dictionary<object, object>()
                    {
                    }));

            Assert.AreEqual(
                "test: 123\n",
                YamlSerializer.Serialize(
                    new Dictionary<string, string>()
                    {
                        { "test", "123" }
                    }));

            Assert.AreEqual(
                "aaa: 123\n" +
                "bbb: 456\n",
                YamlSerializer.Serialize(
                    new SortedDictionary<string, object>()
                    {
                        { "bbb", 456 },
                        { "aaa", 123 }
                    }));
        }

        [TestMethod]
        public void TestIgnoredItems()
        {
            Assert.AreEqual(
                "bbb: 456\n",
                YamlSerializer.Serialize(
                    new Dictionary<string, object>()
                    {
                        { "aaa", null },
                        { "bbb", 456 },
                        { "ccc", new DummyIgnoredItem() }
                    }));
        }

        [TestMethod]
        public void TestNestedMappings()
        {
            Assert.AreEqual(
                "aaa: 123\n" +
                "bbb:\n" +
                "  b1: 4\n" +
                "  b2: 5\n" +
                "ccc: 678\n",
                YamlSerializer.Serialize(
                    new Dictionary<string, object>()
                    {
                        { "aaa", 123 },
                        { "bbb", new Dictionary<string, object>()
                            {
                                { "b1", 4 },
                                { "b2", 5 }
                            }
                        },
                        { "ccc", 678 }
                    }));
        }

        [TestMethod]
        [ExpectedException(typeof(YamlSerializationTooDeep))]
        public void TestMaxDepth()
        {
            Assert.AreEqual(
                "aaa:\n" +
                "  aaa:\n" +
                "    aaa: 1\n",
                YamlSerializer.Serialize(
                    new Dictionary<object, object>()
                    {
                        { "aaa", new Dictionary<object, object>()
                            {
                                { "aaa", new Dictionary<object, object>()
                                    {
                                        { "aaa", 1 }
                                    }
                                }
                            }
                        }
                    },
                    new YamlSerializationOptions
                    {
                        MaxDepth = 3
                    }));

            YamlSerializer.Serialize(
                new Dictionary<object, object>()
                {
                    { "aaa", new Dictionary<object, object>()
                        {
                            { "aaa", new Dictionary<object, object>()
                                {
                                    { "aaa", new Dictionary<object, object>() }
                                }
                            }
                        }
                    }
                },
                new YamlSerializationOptions
                {
                    MaxDepth = 3
                });
        }
    }
}
