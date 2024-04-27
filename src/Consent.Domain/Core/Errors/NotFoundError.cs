namespace Consent.Domain.Core.Errors;

public class NotFoundError : Error
{
    public NotFoundError()
    {
    }

    public NotFoundError(string detail) : base(detail)
    {
    }
}
