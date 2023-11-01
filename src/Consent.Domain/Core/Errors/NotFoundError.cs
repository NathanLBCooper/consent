namespace Consent.Domain.Core.Errors;

public record NotFoundError : Error
{
    public NotFoundError(string? message = null) : base(message)
    {
    }
}
