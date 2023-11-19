using Consent.Domain.Users;

namespace Consent.Application.Users.Get;

public record UserGetQuery(
    UserId UserId,
    UserId RequestedBy
    );
