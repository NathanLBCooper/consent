using System;
using System.Linq;
using Consent.Domain.Workspaces;

namespace Consent.Api.Models;

public record WorkspaceModel(int Id, string Name, WorkspaceMembershipModel[] Memberships);

public record WorkspaceMembershipModel(int UserId, WorkspacePermissionModel[] Permissions);

public enum WorkspacePermissionModel
{
    View,
    Edit,
    Admin,
    Buyer
}

internal static class WorkspaceModelMapper
{
    public static WorkspaceModel ToModel(this WorkspaceEntity entity)
    {
        return new WorkspaceModel(entity.Id.Value, entity.Name, entity.Memberships.Select(m => m.ToModel()).ToArray());
    }

    private static WorkspaceMembershipModel ToModel(this WorkspaceMembership membership)
    {
        return new WorkspaceMembershipModel(membership.UserId.Value, membership.Permissions.Select(p => p.ToModel()).ToArray());
    }

    private static WorkspacePermissionModel ToModel(this WorkspacePermission permission)
    {
        return permission switch
        {
            WorkspacePermission.View => WorkspacePermissionModel.View,
            WorkspacePermission.Edit => WorkspacePermissionModel.Edit,
            WorkspacePermission.Admin => WorkspacePermissionModel.Admin,
            WorkspacePermission.Buyer => WorkspacePermissionModel.Buyer,
            _ => throw new ArgumentException()
        };
    }
}
