using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Stubborn.Tests
{
    [TestClass]
    public class PrimitiveTypes
    {
        [TestMethod]
        public void TestInt()
        {
            Assert.AreEqual(
                "123\n",
                YamlSerializer.Serialize(
                    (object)123));

            Assert.AreEqual(
                "-123\n",
                YamlSerializer.Serialize(
                    (object)-123));
        }

        [TestMethod]
        public void TestLong()
        {
            Assert.AreEqual(
                "456456456456456456\n",
                YamlSerializer.Serialize(
                    (object)456456456456456456L));

            Assert.AreEqual(
                "-456456456456456456\n",
                YamlSerializer.Serialize(
                    (object)-456456456456456456L));
        }

        [TestMethod]
        public void TestDouble()
        {
            Assert.AreEqual(
                "456456.456456\n",
                YamlSerializer.Serialize(
                    (object)456456.456456));

            Assert.AreEqual(
                "-456456.456456\n",
                YamlSerializer.Serialize(
                    (object)-456456.456456));
        }

        [TestMethod]
        public void TestBool()
        {
            Assert.AreEqual(
                "true\n",
                YamlSerializer.Serialize(
                    (object)true));

            Assert.AreEqual(
                "false\n",
                YamlSerializer.Serialize(
                    (object)false));
        }
    }
}
