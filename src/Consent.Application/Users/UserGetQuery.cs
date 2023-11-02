using Consent.Domain.Users;

namespace Consent.Application.Users;

public record UserGetQuery(
    UserId UserId,
    UserId RequestedBy
    );
