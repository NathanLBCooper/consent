namespace Consent.Domain.Core.Errors;

public class UnauthorizedError : Error
{
    public UnauthorizedError()
    {
    }

    public UnauthorizedError(string message) : base(message)
    {
    }
}
