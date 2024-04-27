using Consent.Domain.Contracts;

namespace Consent.Application.Contracts.VersionCreate;

public record ContractVersionCreateCommandResponse(
    Contract Contract,
    ContractVersion Version
    );
