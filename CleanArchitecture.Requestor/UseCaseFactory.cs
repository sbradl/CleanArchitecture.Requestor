using System;
using System.Collections.Generic;

namespace CleanArchitecture.Requestor;

public sealed class UseCaseFactory
{
    private static readonly Lazy<UseCaseFactory>
    LazyInstance = new Lazy<UseCaseFactory>(() => new UseCaseFactory());

    private readonly IDictionary<string, Func<IUseCase>> useCaseFactories = new Dictionary<string, Func<IUseCase>>();
    
    public static UseCaseFactory Instance => LazyInstance.Value;

    public void Register(string name, Func<IUseCase> factoryFunction)
    {
        name = name.ToUpperInvariant();
        
        if (this.useCaseFactories.ContainsKey(name))
        throw new UseCaseAlreadyRegistered(name);
        
        this.useCaseFactories.Add(name, factoryFunction);
    }

    public IUseCase CreateUseCase(string name)
    {
        name = name.ToUpperInvariant();

        if (!this.useCaseFactories.ContainsKey(name))
        throw new UnregisteredUseCase(name);

        return this.useCaseFactories[name]();
    }

    public sealed class UnregisteredUseCase : Exception
    {
        internal UnregisteredUseCase(string useCaseName)
        : base($"Cannot create use case named '{useCaseName}' because it has not been registered.")
        {
            this.RequestedUseCase = useCaseName;
        }

        public string RequestedUseCase { get; }
    }

    public sealed class UseCaseAlreadyRegistered : Exception
    {
        public string UseCaseName { get; }

        internal UseCaseAlreadyRegistered(string useCaseName)
        : base($"Use Case named '{useCaseName}' is already registered.")
        {
            this.UseCaseName = useCaseName;
        }
    }
}
