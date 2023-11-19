using Consent.Domain.Contracts;
using Consent.Domain.Permissions;
using Consent.Domain.Users;

namespace Consent.Application.Contracts.ProvisionCreate;

public record ProvisionCreateCommand(
    string? Text,
    PermissionId[]? PermissionIds,
    ContractVersionId ContractVersionId,
    UserId RequestedBy
    );
