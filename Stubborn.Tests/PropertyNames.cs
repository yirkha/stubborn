using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Stubborn.Tests
{
    class DummyPropertyNames
    {
        public string Upper => "";

        public string lower => "";

        public string UpperCamelCase => "";

        public string lowerCamelCase => "";

        public string Upper_With_Underscores => "";

        public string lower_with_underscores => "";

        [YamlName("cuSTom-nAMe")]
        public string CustomName => "";
    }

    [TestClass]
    public class PropertyNames
    {
        [TestMethod]
        public void TestAsIs()
        {
            Assert.AreEqual(
                "Upper:\n" +
                "lower:\n" +
                "UpperCamelCase:\n" +
                "lowerCamelCase:\n" +
                "Upper_With_Underscores:\n" +
                "lower_with_underscores:\n" +
                "cuSTom-nAMe:\n",
                YamlSerializer.Serialize(
                    new DummyPropertyNames(),
                    new YamlSerializationOptions
                    {
                        PropertyNamingStyle = YamlNamingStyle.AsIs
                    }));
        }

        [TestMethod]
        public void TestUpperCamelCase()
        {
            Assert.AreEqual(
                "Upper:\n" +
                "Lower:\n" +
                "UpperCamelCase:\n" +
                "LowerCamelCase:\n" +
                "UpperWithUnderscores:\n" +
                "LowerWithUnderscores:\n" +
                "cuSTom-nAMe:\n",
                YamlSerializer.Serialize(
                    new DummyPropertyNames(),
                    new YamlSerializationOptions
                    {
                        PropertyNamingStyle = YamlNamingStyle.UpperCamelCase
                    }));
        }

        [TestMethod]
        public void TestLowerCamelCase()
        {
            Assert.AreEqual(
                "upper:\n" +
                "lower:\n" +
                "upperCamelCase:\n" +
                "lowerCamelCase:\n" +
                "upperWithUnderscores:\n" +
                "lowerWithUnderscores:\n" +
                "cuSTom-nAMe:\n",
                YamlSerializer.Serialize(
                    new DummyPropertyNames(),
                    new YamlSerializationOptions
                    {
                        PropertyNamingStyle = YamlNamingStyle.LowerCamelCase
                    }));
        }
    }
}
