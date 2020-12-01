using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Stubborn.Tests
{
    [YamlFormat(BlankLinesAfter = 1)]
    class DummyStep
    {
        [YamlOrder(-100)]
        public string Name { get; set; }

        [YamlIgnore(IfEquals = "previous-succeeded")]
        public string Condition { get; set; }

        public string Type { get; set; }

        [YamlOrder(100)]
        [YamlIgnore(IfEquals = 3600)]
        [YamlComment(Property = "TimeoutSecHumanReadable", Style = YamlCommentStyle.SameLine)]
        public int TimeoutSec { get; set; } = 3600;
        [YamlIgnore]
        public string TimeoutSecHumanReadable => TimeSpan.FromSeconds(TimeoutSec).ToString("c");

        [YamlIgnore(IfEquals = true)]
        public bool Enabled { get; set; } = true;
    }

    class DummyCheckoutStep : DummyStep
    {
        public string Repo { get; set; }
    }

    class DummyShellStep : DummyStep
    {
        [YamlFormat(Block = true)]
        public string Command { get; set; }
    }

    class DummyTestStep : DummyStep
    {
        [YamlFormat(Quoted = true)]
        public string Parameters { get; set; }

        [YamlFormat(AlwaysNested = true)]
        public IEnumerable<string> Checks { get; set; }
    }

    class DummyPublishStep : DummyStep
    {
        public DummyPublishStep()
        {
            Enabled = false;
        }

        public string Mask { get; set; }

        [YamlComment("TODO: Get a cert before publishing", YamlCommentStyle.SameLine)]
        public string SignCert { get; set; }
    }

    class DummyExample
    {
        public string Name => "Build pipeline example";
        public string Version => "0.1";

        [YamlFormat(BlankLinesBefore = 1)]
        [YamlComment("Pool settings", YamlCommentStyle.MediumAbove)]
        public string PoolGroup => "windows";
        public string PoolRegion => "eu";
        [YamlName("cloud-based")]
        public bool CloudBased => true;

        [YamlFormat(BlankLinesBefore = 1)]
        [YamlComment("Variables", YamlCommentStyle.MediumAbove)]
        public IDictionary<string, object> Variables => new Dictionary<string, object>()
        {
            { "Environment", "Test" },
            { "ENABLE_EXPERIMENTAL", "true" }
        };

        [YamlFormat(BlankLinesBefore = 1)]
        [YamlComment("Build", YamlCommentStyle.MediumAbove)]
        public IEnumerable<DummyStep> Build => new List<DummyStep>()
        {
            new DummyCheckoutStep()
            {
                Name = "Checkout",
                Type = "git.checkout",
                Repo = "https://example.com/git/example-repo"
            },
            new DummyShellStep()
            {
                Name = "Prepare environment",
                Type = "shell",
                Command = "tools\\prepare_build.bat $(Environment)"
            },
            new DummyShellStep()
            {
                Name = "Run the build",
                Type = "shell",
                Command =
                    "echo Main build starting\n" +
                    "set ENABLE_SPECIAL_THING=true\n" +
                    "msbuild /p:Configuration=Release Project.sln\n"
            }
        };

        [YamlFormat(BlankLinesBefore = 1)]
        [YamlComment("Test", YamlCommentStyle.MediumAbove)]
        public IEnumerable<DummyStep> Test => new List<DummyStep>()
        {
            new DummyTestStep()
            {
                Name = "Test common",
                Type = "test.run",
                Parameters = "-Tests common/*",
                Checks = new string[] { "AllSucceeded", "MemoryLeaks" }
            },
            new DummyTestStep()
            {
                Name = "Test frontend",
                Type = "test.run",
                Parameters = "-Tests frontend/* -RandomMask #RANDOM#",
                Checks = new string[] { "AllSucceeded" }
            },
            new DummyTestStep()
            {
                Name = "Test backend",
                Type = "test.run",
                TimeoutSec = 4200,
                Parameters = "-Tests backend/*",
                Checks = new string[] { "AllSucceeded", "MemoryLeaks" }
            },
            new DummyShellStep()
            {
                Name = "Clean up",
                Type = "shell",
                Condition = "always",
                Command = "rm -rf /"
            }
        };

        [YamlFormat(BlankLinesBefore = 1)]
        [YamlComment("Publish", YamlCommentStyle.MediumAbove)]
        public IEnumerable<DummyStep> Publish => new List<DummyStep>()
        {
            new DummyPublishStep()
            {
                Name = "Upload package",
                Type = "artifacts.upload",
                Mask = "*.zip",
                SignCert = "0000000000000000000000000000000000000000"
            }
        };

        public static YamlSerializationOptions SerializationOptions = new YamlSerializationOptions
        {
            PrologueComment =
                "This example demonstrates various ways in which extra attributes can be used\n" +
                "to make the output YAML file more readable and maintainable.",
            PrologueCommentStyle = YamlCommentStyle.LargeAbove,

            EpilogueComment = "Now go make your own YAML files pretty :-)"
        };
    }

    [TestClass]
    public class Example
    {
        private string ReadTextFile(string pathFromRepoRoot)
        {
            return
                System.IO.File.ReadAllText(
                    $"../../../../{pathFromRepoRoot}",
                    System.Text.Encoding.UTF8)
                .Replace(Environment.NewLine, "\n");
        }

        [TestMethod]
        public void TestSerializeWithoutAttributes()
        {
            Assert.AreEqual(
                ReadTextFile("example-default.yaml"),
                YamlSerializer.Serialize(
                    new DummyExample(),
                    new YamlSerializationOptions()
                    {
                        DisableOverrides = true
                    }));
        }

        [TestMethod]
        public void TestSerializeWithEverything()
        {
            Assert.AreEqual(
                ReadTextFile("example-stubborn.yaml"),
                YamlSerializer.Serialize(
                    new DummyExample(),
                    DummyExample.SerializationOptions));
        }
    }
}
