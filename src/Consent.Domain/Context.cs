using Consent.Domain.Users;

namespace Consent.Domain;

public record Context
{
    public UserId UserId { get; init; }
}
