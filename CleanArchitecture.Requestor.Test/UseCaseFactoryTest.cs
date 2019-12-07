using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CleanArchitecture.Requestor.Test
{
    [TestClass]
    public sealed class UseCaseFactoryTest
    {
        [TestMethod]
        [ExpectedException(typeof(UseCaseFactory.UnregisteredUseCase))]
        public void GivenUnknownUseCaseName_CreateUseCaseFails()
        {
            UseCaseFactory.Instance.CreateUseCase("unknown use case");
        }

        [TestMethod]
        public void UnregisteredUseCaseError_ContainsRequestedUseCaseName()
        {
            try
            {
                UseCaseFactory.Instance.CreateUseCase("uc1");
            }
            catch (UseCaseFactory.UnregisteredUseCase e)
            {
                StringAssert.Contains(e.Message, "UC1");
                Assert.AreEqual("UC1", e.RequestedUseCase);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(UseCaseFactory.UseCaseAlreadyRegistered))]
        public void RegisterTwice_IsNotAllowed()
        {
            UseCaseFactory.Instance.Register("uc2", () => new UseCase1());
            UseCaseFactory.Instance.Register("uc2", () => new UseCase1());
        }

        [TestMethod]
        [ExpectedException(typeof(UseCaseFactory.UseCaseAlreadyRegistered))]
        public void Register_IgnoresCase()
        {
            UseCaseFactory.Instance.Register("Uc3", () => new UseCase1());
            UseCaseFactory.Instance.Register("uC3", () => new UseCase1());
        }

        [TestMethod]
        public void AlreadyRegisteredUseCaseError_ContainsUseCaseName()
        {
            try
            {
                UseCaseFactory.Instance.Register("uc4", () => new UseCase1());
                UseCaseFactory.Instance.Register("uc4", () => new UseCase1());
            }
            catch (UseCaseFactory.UseCaseAlreadyRegistered e)
            {
                e.Message.Should().Contain("UC4");
                e.UseCaseName.Should().Be("UC4");
            }
        }

        [TestMethod]
        public void GivenRegisteredUseCase_ExecuteReturnsNewInstance()
        {
            UseCaseFactory.Instance.Register("UC5", () => new UseCase1());
            var instance1 = UseCaseFactory.Instance.CreateUseCase("UC5");
            var instance2 = UseCaseFactory.Instance.CreateUseCase("UC5");

            instance1.Should().BeOfType<UseCase1>();
            instance1.Should().NotBeSameAs(instance2);
        }
        
        private sealed class UseCase1 : IUseCase
        {
            public Task Execute(IRequest request)
            {
                return Task.CompletedTask;
            }
        }
    }
}