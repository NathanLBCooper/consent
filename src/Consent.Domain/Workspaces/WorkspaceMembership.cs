using Consent.Domain.Users;

namespace Consent.Domain.Workspaces;

// todo Everything is joined together by Ids. This smells like an abstraction leak from the persistance layer.
//     Consider moving to an ORM with lazy loading to allow more natural joining of entities?

public record WorkspaceMembership
{
    public UserId UserId { get; private init; }
    public WorkspaceId WorkspaceId { get; private init; }
    public WorkspacePermission[] Permissions { get; private init; }

    public WorkspaceMembership(UserId userId, WorkspaceId workspaceId, WorkspacePermission[] permissions)
    {
        UserId = userId;
        WorkspaceId = workspaceId;
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
