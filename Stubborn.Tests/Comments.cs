using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Stubborn.Tests
{
    class DummyComments
    {
        [YamlComment("Lorem ipsum dolor sit amet, consectetur adipiscing elit, " +
            "sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
            YamlCommentStyle.LargeAbove)]
        public int LargeAbove => 0;

        [YamlComment("Vestibulum viverra interdum sapien, eu convallis augue " +
            "pharetra eu. Nam nec diam vitae quam mattis sollicitudin.",
            YamlCommentStyle.MediumAbove)]
        public int MediumAbove => 0;

        [YamlComment("In eleifend nisl urna. Praesent tristique dignissim " +
            "consequat. Nunc quis felis sit amet dolor malesuada luctus.",
            YamlCommentStyle.SmallAbove)]
        public int SmallAbove => 0;

        [YamlComment("Lorem ipsum dolor sit amet", YamlCommentStyle.SameLine)]
        public int SameLine => 0;
    }

    [YamlComment("This is the top-level class", YamlCommentStyle.LargeAbove)]
    class DummyIndentedComments
    {
        public DummyComments Content => new DummyComments();
    }

    class DummyDynamicComments
    {
        [YamlComment("Hello", YamlCommentStyle.SameLine)]
        public int Static => 0;

        [YamlComment("", YamlCommentStyle.SameLine)]
        public int StaticEmpty => 0;

        [YamlComment(Property = "DynamicComment", Style = YamlCommentStyle.SameLine)]
        public int Dynamic => 0;

        [YamlComment(Property = "DynamicEmptyComment", Style = YamlCommentStyle.SameLine)]
        public int DynamicEmpty => 0;

        [YamlComment(Property = "DynamicDisabledComment", Style = YamlCommentStyle.SameLine)]
        public int DynamicDisabled => 0;

        [YamlIgnore]
        public string DynamicComment => "Hi there";

        [YamlIgnore]
        public string DynamicEmptyComment => "";

        [YamlIgnore]
        public string DynamicDisabledComment => null;
    }

    [TestClass]
    public class Comments
    {
        [TestMethod]
        public void TestCommentsStyles()
        {
            Assert.AreEqual(
                "################################################################################\n" +
                "# Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor\n" +
                "# incididunt ut labore et dolore magna aliqua.\n" +
                "################################################################################\n" +
                "largeAbove: 0\n" +
                "#\n" +
                "# Vestibulum viverra interdum sapien, eu convallis augue pharetra eu. Nam nec\n" +
                "# diam vitae quam mattis sollicitudin.\n" +
                "#\n" +
                "mediumAbove: 0\n" +
                "# In eleifend nisl urna. Praesent tristique dignissim consequat. Nunc quis felis\n" +
                "# sit amet dolor malesuada luctus.\n" +
                "smallAbove: 0\n" +
                "sameLine: 0  # Lorem ipsum dolor sit amet\n",
                YamlSerializer.Serialize(
                    new DummyComments()));

            Assert.AreEqual(
                "##################################################\n" +
                "# This is the top-level class\n" +
                "##################################################\n" +
                "content:\n" +
                "  ################################################\n" +
                "  # Lorem ipsum dolor sit amet, consectetur\n" +
                "  # adipiscing elit, sed do eiusmod tempor\n" +
                "  # incididunt ut labore et dolore magna aliqua.\n" +
                "  ################################################\n" +
                "  largeAbove: 0\n" +
                "  #\n" +
                "  # Vestibulum viverra interdum sapien, eu\n" +
                "  # convallis augue pharetra eu. Nam nec diam\n" +
                "  # vitae quam mattis sollicitudin.\n" +
                "  #\n" +
                "  mediumAbove: 0\n" +
                "  # In eleifend nisl urna. Praesent tristique\n" +
                "  # dignissim consequat. Nunc quis felis sit amet\n" +
                "  # dolor malesuada luctus.\n" +
                "  smallAbove: 0\n" +
                "  sameLine: 0  # Lorem ipsum dolor sit amet\n",
                YamlSerializer.Serialize(
                    new DummyIndentedComments(),
                    new YamlSerializationOptions()
                    {
                        Width = 50
                    }));
        }

        [TestMethod]
        public void TestDynamicComment()
        {
            Assert.AreEqual(
                "static: 0  # Hello\n" +
                "staticEmpty: 0  #\n" +
                "dynamic: 0  # Hi there\n" +
                "dynamicEmpty: 0  #\n" +
                "dynamicDisabled: 0\n",
                YamlSerializer.Serialize(
                    new DummyDynamicComments()));
        }
    }
}
