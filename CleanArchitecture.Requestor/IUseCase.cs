using System.Threading.Tasks;

namespace CleanArchitecture.Requestor
{
    public interface IUseCase
    {
        Task Execute(IRequest request);
    }
}