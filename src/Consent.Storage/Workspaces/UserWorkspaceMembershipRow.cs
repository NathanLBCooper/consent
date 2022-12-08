using Consent.Domain.Users;
using Consent.Domain.Workspaces;

namespace Consent.Storage.Workspaces;

internal record UserWorkspaceMembershipRow
{
    public UserId UserId { get; private init; }
    public WorkspaceId WorkspaceId { get; private init; }
    public WorkspacePermission Permission { get; private init; }

    public UserWorkspaceMembershipRow(UserId userId, WorkspaceId workspaceId, WorkspacePermission permission)
    {
        UserId = userId;
        WorkspaceId = workspaceId;
        Permission = permission;
    }
}
