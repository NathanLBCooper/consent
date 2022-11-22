using Consent.Domain.Workspaces;

namespace Consent.Storage.Workspaces
{
    internal record UserWorkspaceMembershipRow
    {
        public int UserId { get; private init; }
        public int WorkspaceId { get; private init; }
        public WorkspacePermission Permission { get; private init; }

        public UserWorkspaceMembershipRow(int userId, int workspaceId, WorkspacePermission permission)
        {
            UserId = userId;
            WorkspaceId = workspaceId;
            Permission = permission;
        }
    }
}
