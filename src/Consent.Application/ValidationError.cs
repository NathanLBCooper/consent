using Consent.Domain.Core;

namespace Consent.Application;

public record ValidationError : Error
{
    public ValidationError(string message) : base(message)
    {
    }
}
