using Consent.Domain.Contracts;

namespace Consent.Application.Contracts.VersionCreate;

public record ContractVersionCreateCommandResult(
    Contract Contract,
    ContractVersion Version
    );
