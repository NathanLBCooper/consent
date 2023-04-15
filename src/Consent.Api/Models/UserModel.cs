using System.Linq;
using Consent.Domain.Users;

namespace Consent.Api.Models;

public record UserModel
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public UserWorkspaceMembershipModel[]? WorkspaceMemberships { get; init; }
}

public record UserWorkspaceMembershipModel(int WorkspaceId, WorkspacePermissionModel[] Permissions);

internal static class UserModelMapper
{
    public static UserModel ToModel(this User entity)
    {
        return new UserModel
        {
            Id = entity.Id!.Value.Value,
            Name = entity.Name,
            WorkspaceMemberships = entity.WorkspaceMemberships.Select(ToModel).ToArray()
        };
    }

    public static UserWorkspaceMembershipModel ToModel(this WorkspaceMembership entity)
    {
        return new UserWorkspaceMembershipModel(entity.WorkspaceId.Value,
            entity.Permissions.Select(p => p.ToModel()).ToArray()
            );
    }
}
