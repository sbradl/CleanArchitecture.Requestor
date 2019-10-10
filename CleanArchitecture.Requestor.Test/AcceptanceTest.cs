using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CleanArchitecture.Requestor.Test
{
    [TestClass]
    public sealed class AcceptanceTest
    {
        private string addedUser = string.Empty;
        
        public AcceptanceTest()
        {
            RequestBuilder.Instance.Register("add_user", AddUserRequestBuilder.BuildAddUserRequestFrom);
            UseCaseFactory.Instance.Register("add_user", () => new AddUserUseCase(n => addedUser = n));
        }
        
        [TestMethod]
        public void ExecuteUseCase()
        {
            var request = BuildRequest();
            
            ExecuteUseCase();
            
            Assert.AreEqual("Stefan", this.addedUser);
            

            IRequest BuildRequest()
            {
                return RequestBuilder.Instance.BuildRequest("add_user", new Dictionary<string, object>
                {
                    {"user_name", "Stefan"}
                });
            }
            
            void ExecuteUseCase()
            {
                UseCaseFactory.Instance
                    .CreateUseCase("add_user")
                    .Execute(request);
            }
        }

        private static class AddUserRequestBuilder
        {
            public static IRequest BuildAddUserRequestFrom(IDictionary<string, object> properties)
            {
                return new AddUserRequest
                {
                    UserName = (string) properties["user_name"]
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
            
            public void Execute(IRequest request)
            {
                var r = (AddUserRequest)request;
                this.onUserAdded(r.UserName);
            }
        }
    }
}