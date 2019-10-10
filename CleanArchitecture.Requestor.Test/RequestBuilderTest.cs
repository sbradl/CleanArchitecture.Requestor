using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CleanArchitecture.Requestor.Test
{
    [TestClass]
    public sealed class RequestBuilderTest
    {
        [TestMethod]
        [ExpectedException(typeof(RequestBuilder.UnregisteredRequest))]
        public void GivenUnknownRequestName_BuildRequestFails()
        {
            RequestBuilder.Instance.BuildRequest("unknown request name", null);
        }

        [TestMethod]
        public void UnregisteredRequestError_ContainsRequestName()
        {
            try
            {
                RequestBuilder.Instance.BuildRequest("req1", null);
            }
            catch (RequestBuilder.UnregisteredRequest e)
            {
                StringAssert.Contains(e.Message, "REQ1");
                Assert.AreEqual("REQ1", e.RequestName);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(RequestBuilder.RequestAlreadyRegistered))]
        public void RegisterTwice_IsNotAllowed()
        {
            RequestBuilder.Instance.Register("REQ2", _ => new Request());
            RequestBuilder.Instance.Register("REQ2", _ => new Request());
        }

        [TestMethod]
        [ExpectedException(typeof(RequestBuilder.RequestAlreadyRegistered))]
        public void Register_IgnoresCase()
        {
            RequestBuilder.Instance.Register("REQ2", _ => new Request());
            RequestBuilder.Instance.Register("req2", _ => new Request());
        }
        
        [TestMethod]
        public void AlreadyRegisteredRequestError_ContainsRequestName()
        {
            try
            {
                RequestBuilder.Instance.Register("req3", _ => new Request());
                RequestBuilder.Instance.Register("req3", _ => new Request());
            }
            catch (RequestBuilder.RequestAlreadyRegistered e)
            {
                StringAssert.Contains(e.Message, "REQ3");
                Assert.AreEqual("REQ3", e.RequestName);
            }
        }

        [TestMethod]
        public void GivenRegisteredRequest_BuildRequestReturnsNewInstance()
        {
            RequestBuilder.Instance.Register("REQ4", _ => new Request());
            var instance1 = RequestBuilder.Instance.BuildRequest("REQ4", null);
            var instance2 = RequestBuilder.Instance.BuildRequest("REQ4", null);
            
            Assert.IsInstanceOfType(instance1, typeof(Request));
            Assert.AreNotSame(instance1, instance2);
        }

        [TestMethod]
        public void BuildRequest_PassesPropertiesToRegisteredBuilder()
        {
            RequestProperties passedProperties = null;
            var propertiesToPass = new RequestProperties();
            RequestBuilder.Instance.Register("REQ5", p => { passedProperties = p; return new Request(); });

            RequestBuilder.Instance.BuildRequest("req5", propertiesToPass);
            
            Assert.AreSame(propertiesToPass, passedProperties);
        }
        
        private sealed class Request : IRequest
        {
        }
    }
}