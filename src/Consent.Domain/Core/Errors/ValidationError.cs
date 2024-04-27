namespace Consent.Domain.Core.Errors;

public class ValidationError : Error
{
    public ValidationError()
    {
    }

    public ValidationError(string message) : base(message)
    {
    }
}
