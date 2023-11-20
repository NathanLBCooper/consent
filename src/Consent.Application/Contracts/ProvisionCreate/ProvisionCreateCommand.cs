using Consent.Domain.Contracts;
using Consent.Domain.Purposes;
using Consent.Domain.Users;

namespace Consent.Application.Contracts.ProvisionCreate;

public record ProvisionCreateCommand(
    string? Text,
    PurposeId[]? PurposeIds,
    ContractVersionId ContractVersionId,
    UserId RequestedBy
    );
