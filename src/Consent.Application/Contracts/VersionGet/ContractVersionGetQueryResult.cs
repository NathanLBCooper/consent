using Consent.Domain.Contracts;

namespace Consent.Application.Contracts.VersionGet;

public record ContractVersionGetQueryResult(
    Contract Contract,
    ContractVersion Version
    );
