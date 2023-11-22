using Consent.Domain.Users;
using Consent.Domain.Workspaces;

namespace Consent.Application.Purposes.Create;

public record PurposeCreateCommand(
    string? Name,
    string? Description,
    WorkspaceId WorkspaceId,
    UserId RequestedBy
    );
