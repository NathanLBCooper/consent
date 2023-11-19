using Consent.Domain.Contracts;
using Consent.Domain.Users;

namespace Consent.Application.Contracts.VersionGet;

public record ContractVersionGetQuery(
    ContractVersionId ContractVersionId,
    UserId RequestedBy
    );
