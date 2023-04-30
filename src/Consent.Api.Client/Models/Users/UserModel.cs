using Consent.Api.Client.Models.Workspaces;

namespace Consent.Api.Client.Models.Users;

public record UserModel(
    int Id,
    string? Name,
    UserWorkspaceMembershipModel[]? WorkspaceMemberships
    );

public record UserWorkspaceMembershipModel(
    ResourceLink Workspace,
    WorkspacePermissionModel[] Permissions
    );
