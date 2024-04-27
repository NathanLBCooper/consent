namespace Consent.Domain.Core.Errors;

public record InvalidOperationError : Error
{
    public InvalidOperationError(string message) : base(message)
    {
    }
}
