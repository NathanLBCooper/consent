using System.Linq;
using Consent.Api.Models.Workspaces;
using Consent.Domain;
using Consent.Domain.Users;

namespace Consent.Api.Models.Users;

public record UserModel(
    int Id,
    string? Name,
    UserWorkspaceMembershipModel[]? WorkspaceMemberships
    );

public record UserWorkspaceMembershipModel(
    ResourceLink Workspace,
    WorkspacePermissionModel[] Permissions
    );

internal static class UserModelMapper
{
    public static UserModel ToModel(this User entity, ConsentLinkGenerator linkGenerator)
    {
        return new UserModel(
            Id: Guard.NotNull(entity.Id).Value.Value,
            Name: entity.Name,
            WorkspaceMemberships: entity.WorkspaceMemberships.Select(m => m.ToModel(linkGenerator)).ToArray()
            );
    }

    public static UserWorkspaceMembershipModel ToModel(this WorkspaceMembership entity, ConsentLinkGenerator linkGenerator)
    {
        var workspaceId = entity.WorkspaceId;
        var @ref = new ResourceLink(workspaceId.Value, linkGenerator.GetWorkspace(workspaceId));

        return new UserWorkspaceMembershipModel(@ref,
            entity.Permissions.Select(p => p.ToModel()).ToArray()
            );
    }
}
