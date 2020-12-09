using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Stubborn.Tests
{
    [TestClass]
    public class Attributes
    {
        [TestMethod]
        public void TestAccessors()
        {
            var attr = new YamlFormatAttribute();
            Assert.ThrowsException<NotImplementedException>(() => attr.ToString);
            Assert.ThrowsException<NotImplementedException>(() => attr.BlankLinesBefore);
            Assert.ThrowsException<NotImplementedException>(() => attr.BlankLinesAfter);
            Assert.ThrowsException<NotImplementedException>(() => attr.AlwaysNested);
            Assert.ThrowsException<NotImplementedException>(() => attr.Quoted);
            Assert.ThrowsException<NotImplementedException>(() => attr.DoubleQuoted);
            Assert.ThrowsException<NotImplementedException>(() => attr.Block);
            Assert.ThrowsException<NotImplementedException>(() => attr.IndentStep);
        }
    }
}
