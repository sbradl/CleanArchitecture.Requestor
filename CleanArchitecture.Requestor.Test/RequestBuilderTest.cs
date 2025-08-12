using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CleanArchitecture.Requestor.Test;

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
            e.Message.Should().Contain("REQ1");
            e.RequestName.Should().Be("REQ1");
        }
    }

    [TestMethod]
    [ExpectedException(typeof(RequestBuilder.RequestAlreadyRegistered))]
    public void RegisterTwice_IsNotAllowed()
    {
        RequestBuilder.Instance.Register("REQ2", () => new TestRequestBuilder());
        RequestBuilder.Instance.Register("REQ2", () => new TestRequestBuilder());
    }

    [TestMethod]
    [ExpectedException(typeof(RequestBuilder.RequestAlreadyRegistered))]
    public void Register_IgnoresCase()
    {
        RequestBuilder.Instance.Register("REQ2", () => new TestRequestBuilder());
        RequestBuilder.Instance.Register("req2", () => new TestRequestBuilder());
    }

    [TestMethod]
    public void AlreadyRegisteredRequestError_ContainsRequestName()
    {
        try
        {
            RequestBuilder.Instance.Register("req3", () => new TestRequestBuilder());
            RequestBuilder.Instance.Register("req3", () => new TestRequestBuilder());
        }
        catch (RequestBuilder.RequestAlreadyRegistered e)
        {
            e.Message.Should().Contain("REQ3");
            e.RequestName.Should().Be("REQ3");
        }
    }

    [TestMethod]
    public void GivenRegisteredRequest_BuildRequestReturnsNewInstance()
    {
        RequestBuilder.Instance.Register("REQ4", () => new TestRequestBuilder());
        var instance1 = RequestBuilder.Instance.BuildRequest("REQ4", null);
        var instance2 = RequestBuilder.Instance.BuildRequest("REQ4", null);

        instance1.Should().BeOfType<TestRequest>();
        instance1.Should().NotBeSameAs(instance2);
    }

    [TestMethod]
    public void BuildRequest_PassesPropertiesToRegisteredBuilder()
    {
        var builder = new TestRequestBuilder();
        
        var propertiesToPass = new RequestProperties();
        RequestBuilder.Instance.Register("REQ5", () => builder);

        RequestBuilder.Instance.BuildRequest("req5", propertiesToPass);

        propertiesToPass.Should().BeSameAs(builder.PassedProperties);
    }

    private sealed class TestRequestBuilder : IRequestBuilder
    {
        public RequestProperties PassedProperties { get; private set; }
        
        public IRequest BuildRequestFrom(RequestProperties requestProperties)
        {
            this.PassedProperties = requestProperties;
            
            return new TestRequest();
        }
    }

    private sealed class TestRequest : IRequest
    {
    }
}