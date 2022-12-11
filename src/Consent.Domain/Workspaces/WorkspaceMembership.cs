using System.Linq;
using Consent.Domain.Users;

namespace Consent.Domain.Workspaces;

public class WorkspaceMembership
{
    public static WorkspacePermission[] SuperUser { get; } = new[] {
        WorkspacePermission.View,
        WorkspacePermission.Edit,
        WorkspacePermission.Admin,
        WorkspacePermission.Buyer
    };

    public UserId UserId { get; }
    public WorkspacePermission[] Permissions { get; }
    public bool IsSuperUser { get; }

    public WorkspaceMembership(UserId userId, WorkspacePermission[] permissions)
    {
        UserId = userId;
        Permissions = permissions;

        IsSuperUser = SuperUser.All(p => Permissions.Contains(p));
    }
}

public enum WorkspacePermission
{
    View,
    Edit,
    Admin,
    Buyer
}
