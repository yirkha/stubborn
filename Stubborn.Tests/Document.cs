using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Stubborn.Tests
{
    [TestClass]
    public class Document
    {
        [TestMethod]
        public void TestPrologue()
        {
            Assert.AreEqual(
                "# This is a generated file\n" +
                "\n" +
                "hello: there\n",
                YamlSerializer.Serialize(
                    new Dictionary<string, string>
                    {
                        { "hello", "there" }
                    },
                    new YamlSerializationOptions
                    {
                        PrologueComment = "This is a generated file"
                    }));

            Assert.AreEqual(
                "# This is a generated file\n" +
                "hello: there\n",
                YamlSerializer.Serialize(
                    new Dictionary<string, string>
                    {
                        { "hello", "there" }
                    },
                    new YamlSerializationOptions
                    {
                        PrologueComment = "This is a generated file",
                        PrologueMargin = 0
                    })); ;

            Assert.AreEqual(
                "# This is a generated file\n" +
                "\n" +
                "\n" +
                "hello: there\n",
                YamlSerializer.Serialize(
                    new Dictionary<string, string>
                    {
                        { "hello", "there" }
                    },
                    new YamlSerializationOptions
                    {
                        PrologueComment = "This is a generated file",
                        PrologueMargin = 2
                    })); ;

            Assert.AreEqual(
                "#\n" +
                "# This is a generated file\n" +
                "#\n" +
                "\n",
                YamlSerializer.Serialize(
                    null,
                    new YamlSerializationOptions
                    {
                        PrologueComment = "This is a generated file",
                        PrologueCommentStyle = YamlCommentStyle.MediumAbove
                    }));
        }

        [TestMethod]
        public void TestEpilogue()
        {
            Assert.AreEqual(
                "hello: there\n\n# Continue as needed...\n",
                YamlSerializer.Serialize(
                    new Dictionary<string, string>
                    {
                        { "hello", "there" }
                    },
                    new YamlSerializationOptions
                    {
                        EpilogueComment = "Continue as needed..."
                    }));

            Assert.AreEqual(
                "hello: there\n" +
                "# Continue as needed...\n",
                YamlSerializer.Serialize(
                    new Dictionary<string, string>
                    {
                        { "hello", "there" }
                    },
                    new YamlSerializationOptions
                    {
                        EpilogueComment = "Continue as needed...",
                        EpilogueMargin = 0
                    }));

            Assert.AreEqual(
                "hello: there\n" +
                "\n" +
                "\n" +
                "# Continue as needed...\n",
                YamlSerializer.Serialize(
                    new Dictionary<string, string>
                    {
                        { "hello", "there" }
                    },
                    new YamlSerializationOptions
                    {
                        EpilogueComment = "Continue as needed...",
                        EpilogueMargin = 2
                    }));

            Assert.AreEqual(
                "\n" +
                "#\n" +
                "# Continue as needed...\n" +
                "#\n",
                YamlSerializer.Serialize(
                    null,
                    new YamlSerializationOptions
                    {
                        EpilogueComment = "Continue as needed...",
                        EpilogueCommentStyle = YamlCommentStyle.MediumAbove
                    }));
        }

        [TestMethod]
        public void TestDocumentBoundaries()
        {
            Assert.AreEqual(
                "---\n" +
                "...\n",
                YamlSerializer.Serialize(
                    null,
                    new YamlSerializationOptions
                    {
                        DocumentBoundaries = true
                    }));

            Assert.AreEqual(
                "---\n" +
                "hello: there\n" +
                "...\n",
                YamlSerializer.Serialize(
                    new Dictionary<string, string>
                    {
                        { "hello", "there" }
                    },
                    new YamlSerializationOptions
                    {
                        DocumentBoundaries = true
                    }));
        }
    }
}
