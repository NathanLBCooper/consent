using Consent.Domain.Users;
using Consent.Domain.Workspaces;

namespace Consent.Application.Contracts;

public record ContractCreateCommand(
    string? Name,
    WorkspaceId WorkspaceId,
    UserId RequestedBy
    );
