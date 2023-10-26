using Consent.Domain.Core;

namespace Consent.Application;

public record NotFoundError : Error
{
    public NotFoundError(string? message = null) : base(message ?? string.Empty)
    {
    }
}
