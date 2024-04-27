using Consent.Domain.Contracts;

namespace Consent.Application.Contracts.VersionGet;

public record ContractVersionGetQueryResponse(
    Contract Contract,
    ContractVersion Version
    );
