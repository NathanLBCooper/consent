using Consent.Domain.Users;

namespace Consent.Domain.Workspaces;

public class WorkspaceMembership
{
    public UserId UserId { get; private init; }
    public WorkspacePermission[] Permissions { get; private init; }

    public WorkspaceMembership(UserId userId, WorkspacePermission[] permissions)
    {
        UserId = userId;
        Permissions = permissions;
    }
}

public enum WorkspacePermission
{
    View,
    Edit,
    Admin,
    Buyer
}
