using Consent.Domain.Contracts;

namespace Consent.Application.Contracts.ProvisionCreate;

public record ProvisionCreateCommandResult(
    ContractVersion Version,
    Provision Provision
    );
