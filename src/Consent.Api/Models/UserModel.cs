using System.Linq;
using Consent.Api.Controllers;
using Consent.Domain.Users;

namespace Consent.Api.Models;

public record UserModel
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public UserWorkspaceMembershipModel[]? WorkspaceMemberships { get; init; }
}

public record UserWorkspaceMembershipModel(
    WorkspaceRef Workspace,
    WorkspacePermissionModel[] Permissions
    );

public record WorkspaceRef(
    int Id,
    string? Href
    );

internal static class UserModelMapper
{
    public static UserModel ToModel(this User entity, ConsentLinkGenerator linkGenerator)
    {
        return new UserModel
        {
            Id = entity.Id!.Value.Value,
            Name = entity.Name,
            WorkspaceMemberships = entity.WorkspaceMemberships.Select(m => ToModel(m, linkGenerator)).ToArray()
        };
    }

    public static UserWorkspaceMembershipModel ToModel(this WorkspaceMembership entity, ConsentLinkGenerator linkGenerator)
    {
        var workspaceId = entity.WorkspaceId.Value;
        var @ref = new WorkspaceRef(workspaceId, linkGenerator.GetWorkspace(workspaceId));

        return new UserWorkspaceMembershipModel(@ref,
            entity.Permissions.Select(p => p.ToModel()).ToArray()
            );
    }
}
