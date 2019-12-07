namespace CleanArchitecture.Requestor
{
    public interface IRequestBuilder
    {
        IRequest BuildRequestFrom(RequestProperties requestProperties);
    }
}