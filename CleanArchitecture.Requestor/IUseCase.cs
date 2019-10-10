namespace CleanArchitecture.Requestor
{
    public interface IUseCase
    {
        void Execute(IRequest request);
    }
}