using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CleanArchitecture.Requestor.Test
{
    [TestClass]
    public sealed class RequestPropertiesTest
    {
        [TestMethod]
        public void SetBoolean()
        {
            new RequestProperties()
                .Set("key", true)
                .GetBoolean("keY").Should().BeTrue();
        }

        [TestMethod]
        public void SetString()
        {
            new RequestProperties()
                .Set("key", "value")
                .GetString("keY").Should().Be("value");
        }

        [TestMethod]
        public void SetInt()
        {
            new RequestProperties()
                .Set("key", 42)
                .GetInt("keY").Should().Be(42);
        }

        [TestMethod]
        public void SetDouble()
        {
            new RequestProperties()
                .Set("key", 42.0)
                .GetDouble("keY").Should().Be(42.0);
        }

        [TestMethod]
        public void GivenMissingProperty_GetFails()
        {
            Action tryToGetProperty = () => new RequestProperties().GetString("unknown-key");

            tryToGetProperty.Should().Throw<RequestProperties.UnknownRequestProperty>()
                .And.PropertyName.Should().Be("UNKNOWN-KEY");
        }
    }
}