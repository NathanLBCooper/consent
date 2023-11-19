using Consent.Domain.Users;
using Consent.Domain.Workspaces;

namespace Consent.Application.Contracts.Create;

public record ContractCreateCommand(
    string? Name,
    WorkspaceId WorkspaceId,
    UserId RequestedBy
    );
