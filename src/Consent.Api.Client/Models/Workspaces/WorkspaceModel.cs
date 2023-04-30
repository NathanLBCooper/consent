namespace Consent.Api.Client.Models.Workspaces;

public record WorkspaceModel(
    int Id,
    string Name,
    MembershipModel[] Memberships
    );

public record MembershipModel(
    int UserId,
    WorkspacePermissionModel[] Permissions
    );

public enum WorkspacePermissionModel
{
    View,
    Edit,
    Admin,
    Buyer
}
