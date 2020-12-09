using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace Stubborn.Tests
{
    class DummyObject
    {
        public object Alpha { get; set; }

        public object Beta { get; set; }

        [YamlFormat(IndentStep = 4)]
        [YamlFormat(ToString = false)]
        public object Gamma { get; set; }

        [YamlIgnore]
        public object IgnoreMe => "bad";

        [YamlIgnore(IfEquals = true)]
        [YamlIgnore(IfEqualsStr = "never gonna happen")]
        public object MeToo => true;

        [YamlIgnore(IfEqualsStr = "True")]
        public object Me2 => true;

        [YamlIgnore(IfEmpty = true)]
        public object MeTwo => new List<string>();

        public object MeAsWell => new DummyIgnoredItem();
    }

    class DummyCustomFormat
    {
        [YamlFormat(ToString = true)]
        public DateTime Default => new DateTime(2019, 6, 20, 12, 34, 56);

        [YamlFormat("u")]
        public DateTime Formatted => new DateTime(2019, 6, 20, 12, 34, 56);
    }

    class DummyParentOrderedProperties
    {
        [YamlOrder(-100, ThisOnly = true)]
        public int PA => -1;

        public int PC => -2;

        [YamlOrder(0)]
        public int PB => -3;

        [YamlOrder(100, ThisOnly = true)]
        public int PD => -4;
    }

    class DummyOrderedProperties : DummyParentOrderedProperties
    {
        public int C => 1;

        [YamlOrder(3, ThisOnly = true)]
        public int E => 2;

        public int D => 3;
        public int F => 4;
        public int G => 5;

        [YamlOrder(999)]
        public int I => 6;

        [YamlOrder(-5)]
        public int A => 7;
        public int B => 8;

        [YamlOrder(20)]
        public int H => 9;
    }

    [YamlFormat(BlankLinesBefore = 1, BlankLinesAfter = 1)]
    class DummyFatParent
    {
        public override string ToString() => "meh";
    }

    [YamlFormat(BlankLinesBefore = 0, BlankLinesAfter = 0)]
    class DummySlimChild : DummyFatParent
    {
        public override string ToString() => "heh";
    }

    [TestClass]
    public class Objects
    {
        [TestMethod]
        public void TestObjects()
        {
            Assert.AreEqual(
                "alpha: 1\n" +
                "beta: b\n" +
                "gamma:\n" +
                "- c\n" +
                "- c\n" +
                "- c\n",
                YamlSerializer.Serialize(
                    new DummyObject()
                    {
                        Alpha = 1,
                        Beta = new List<string> { "b" },
                        Gamma = new List<string> { "c", "c", "c" }
                    }));
        }

        [TestMethod]
        public void TestNestedObjects()
        {
            Assert.AreEqual(
                "alpha:\n" +
                "  alpha: 1\n" +
                "  beta: 2\n" +
                "  gamma: 3\n" +
                "beta:\n" +
                "  alpha:\n" +
                "    alpha: a\n" +
                "  beta:\n" +
                "    beta: b\n" +
                "gamma:\n" +
                "    gamma:\n" +
                "        gamma:\n",
                YamlSerializer.Serialize(new DummyObject()
                    {
                        Alpha = new DummyObject()
                        {
                            Alpha = 1,
                            Beta = 2,
                            Gamma = 3
                        },
                        Beta = new DummyObject()
                        {
                            Alpha = new DummyObject()
                            {
                                Alpha = "a"
                            },
                            Beta = new DummyObject()
                            {
                                Beta = "b"
                            }
                        },
                        Gamma = new DummyObject()
                        {
                            Gamma = new DummyObject()
                            {
                                Gamma = new DummyObject()
                            }
                        }
                    }));

            Assert.AreEqual(
                "alpha:\n" +
                "- a\n" +
                "- alpha: 1\n" +
                "- a: 1\n" +
                "beta:\n" +
                "   b:\n" +
                "   - 2\n" +
                "   - 2\n" +
                "   bb:\n" +
                "      beta: 2\n" +
                "gamma: end\n",
                YamlSerializer.Serialize(new DummyObject()
                {
                    Alpha = new List<object>()
                    {
                        "a",
                        new DummyObject()
                        {
                            Alpha = 1
                        },
                        new Dictionary<string, object>()
                        {
                            { "a", 1 }
                        }
                    },
                    Beta = new Dictionary<string, object>()
                    {
                        { "b", new List<object>
                            {
                                2,
                                2
                            }
                        },
                        { "bb", new DummyObject()
                            {
                                Beta = 2
                            }
                        }
                    },
                    Gamma = "end"
                },
                new YamlSerializationOptions()
                {
                    IndentStep = 3
                }));
        }

        [TestMethod]
        public void TestCustomFormat()
        {
            var prevCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            try
            {
                Assert.AreEqual(
                    "default: 06/20/2019 12:34:56\n" +
                    "formatted: 2019-06-20 12:34:56Z\n",
                    YamlSerializer.Serialize(new DummyCustomFormat()));
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = prevCulture;
            }
        }

        [TestMethod]
        public void TestPropertyOrder()
        {
            Assert.AreEqual(
                "pA: -1\n" +
                "a: 7\n" +
                "b: 8\n" +
                "c: 1\n" +
                "pB: -3\n" +
                "d: 3\n" +
                "e: 2\n" +
                "f: 4\n" +
                "g: 5\n" +
                "h: 9\n" +
                "pC: -2\n" +
                "pD: -4\n" +
                "i: 6\n",
                YamlSerializer.Serialize(new DummyOrderedProperties()));
        }

        [TestMethod]
        public void TestParentOverride()
        {
            Assert.AreEqual(
                "\n" +
                "- meh\n" +
                "\n" +
                "- meh\n" +
                "\n" +
                "- heh\n" +
                "- heh\n",
                YamlSerializer.Serialize(new List<object>()
                {
                    new DummyFatParent(),
                    new DummyFatParent(),
                    new DummySlimChild(),
                    new DummySlimChild()
                }));
        }

        [TestMethod]
        public void TestMaxDepth()
        {
            Assert.AreEqual(
                "alpha:\n" +
                "  alpha:\n" +
                "    alpha: 1\n",
                YamlSerializer.Serialize(
                    new DummyObject()
                    {
                        Alpha = new DummyObject()
                        {
                            Alpha = new DummyObject()
                            {
                                Alpha = 1
                            }
                        }
                    },
                    new YamlSerializationOptions
                    {
                        MaxDepth = 3
                    }));

            Assert.ThrowsException<YamlSerializationTooDeep>(() =>
                YamlSerializer.Serialize(
                    new DummyObject()
                    {
                        Alpha = new DummyObject()
                        {
                            Alpha = new DummyObject()
                            {
                                Alpha = new DummyObject()
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
