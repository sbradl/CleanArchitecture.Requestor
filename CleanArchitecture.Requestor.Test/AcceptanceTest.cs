using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CleanArchitecture.Requestor.Test;

[TestClass]
public sealed class AcceptanceTest
{
    private string addedUser = string.Empty;
    
    public AcceptanceTest()
    {
        RequestBuilder.Instance.Register("add_user", () => new AddUserRequestBuilder());
        UseCaseFactory.Instance.Register("add_user", () => new AddUserUseCase(n => this.addedUser = n));
    }
    
    [TestMethod]
    public void ExecuteUseCase()
    {
        var request = BuildRequest();
        
        ExecuteUseCase();
        
        this.addedUser.Should().Be("Stefan");
        

        IRequest BuildRequest()
        {
            return RequestBuilder.Instance.BuildRequest("add_user", new RequestProperties()
                .Set("user_name", "Stefan"));
        }
        
        void ExecuteUseCase()
        {
            UseCaseFactory.Instance
            .CreateUseCase("add_user")
            .Execute(request);
        }
    }

    private sealed class AddUserRequestBuilder : IRequestBuilder
    {
        public IRequest BuildRequestFrom(RequestProperties properties)
        {
            return new AddUserRequest
            {
                UserName = properties.GetString("user_name")
            };
        } 
    }

    private sealed class AddUserRequest : IRequest
    {
        public string UserName;
    }

    private sealed class AddUserUseCase : IUseCase
    {
        private readonly Action<string> onUserAdded;

        public AddUserUseCase(Action<string> onUserAdded)
        {
            this.onUserAdded = onUserAdded;
        }
        
        public Task Execute(IRequest request)
        {
            var r = (AddUserRequest)request;
            this.onUserAdded(r.UserName);
            
            return Task.CompletedTask;
        }
    }
}