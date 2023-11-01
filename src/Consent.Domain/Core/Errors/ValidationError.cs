namespace Consent.Domain.Core.Errors;

public record ValidationError : Error
{
    public ValidationError(string message) : base(message)
    {
    }
}
