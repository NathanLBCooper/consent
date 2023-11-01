namespace Consent.Domain.Core.Errors;

public record UnauthorizedError : Error
{
    public UnauthorizedError(string? message = null) : base(message)
    {
    }
}
