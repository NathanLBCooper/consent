using Consent.Domain.Purposes;
using Consent.Domain.Users;

namespace Consent.Application.Purposes.Get;

public record PurposeGetQuery(
    PurposeId PurposeId,
    UserId RequestedBy
    );
