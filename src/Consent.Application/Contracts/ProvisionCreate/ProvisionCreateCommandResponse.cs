using Consent.Domain.Contracts;

namespace Consent.Application.Contracts.ProvisionCreate;

public record ProvisionCreateCommandResponse(
    ContractVersion Version,
    Provision Provision
    );
