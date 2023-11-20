namespace Consent.Api.Client.Models.Workspaces;

public record WorkspaceModel(
    int Id,
    string Name,
    MembershipModel[] Memberships
    );

public record MembershipModel(
    int UserId,
    PermissionModel[] Permissions
    );

public enum PermissionModel
{
    View,
    Edit,
    Admin,
    Buyer
}
