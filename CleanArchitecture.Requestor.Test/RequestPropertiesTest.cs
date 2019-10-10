using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CleanArchitecture.Requestor.Test
{
    [TestClass]
    public sealed class RequestPropertiesTest
    {
        [TestMethod]
        public void SetBoolean()
        {
            Assert.IsTrue(new RequestProperties()
                .Set("key", true)
                .GetBoolean("keY"));
        }

        [TestMethod]
        public void SetString()
        {
            Assert.AreEqual("value", new RequestProperties()
                .Set("key", "value")
                .GetString("keY"));
        }

        [TestMethod]
        public void SetInt()
        {
            Assert.AreEqual(42, new RequestProperties()
                .Set("key", 42)
                .GetInt("keY"));
        }

        [TestMethod]
        public void SetDouble()
        {
            Assert.AreEqual(42.0, new RequestProperties()
                .Set("key", 42.0)
                .GetDouble("keY"), double.Epsilon);
        }
    }
}