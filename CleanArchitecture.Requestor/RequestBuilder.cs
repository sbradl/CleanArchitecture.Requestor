using System;
using System.Collections.Generic;

namespace CleanArchitecture.Requestor
{
    public sealed class RequestBuilder
    {
        private static readonly Lazy<RequestBuilder>
            LazyInstance = new Lazy<RequestBuilder>(() => new RequestBuilder());

        private readonly IDictionary<string, Func<RequestProperties, IRequest>> requestBuilders =
            new Dictionary<string, Func<RequestProperties, IRequest>>();

        public static RequestBuilder Instance => LazyInstance.Value;

        public void Register(string requestName, Func<RequestProperties, IRequest> requestBuilder)
        {
            requestName = requestName.ToUpperInvariant();

            if (this.requestBuilders.ContainsKey(requestName))
                throw new RequestAlreadyRegistered(requestName);
            
            this.requestBuilders.Add(requestName, requestBuilder);
        }

        public IRequest BuildRequest(string name, RequestProperties properties)
        {
            name = name.ToUpperInvariant();
            
            if (!this.requestBuilders.ContainsKey(name))
                throw new UnregisteredRequest(name);

            return this.requestBuilders[name](properties);
        }

        public sealed class UnregisteredRequest : Exception
        {
            public string RequestName { get; }

            internal UnregisteredRequest(string requestName)
                : base($"Cannot create request named '{requestName}' because it is not registered.")
            {
                this.RequestName = requestName;
            }
        }

        public sealed class RequestAlreadyRegistered : Exception
        {
            public string RequestName { get; }

            internal RequestAlreadyRegistered(string requestName)
                : base($"Request named '{requestName}' is already registered")
            {
                this.RequestName = requestName;
            }
        }
    }
}