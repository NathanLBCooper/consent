using System;
using System.Linq;
using Consent.Api.Client.Models.Workspaces;
using Consent.Domain;
using Consent.Domain.Workspaces;

namespace Consent.Api.Workspaces;

internal static class WorkspaceModelMapper
{
    public static WorkspaceModel ToModel(this Workspace entity)
    {
        var id = Guard.NotNull(entity.Id).Value;
        return new WorkspaceModel(id, entity.Name, entity.Memberships.Select(m => m.ToModel()).ToArray());
    }

    public static MembershipModel ToModel(this Membership membership)
    {
        return new MembershipModel(membership.UserId.Value, membership.Permissions.Select(p => p.ToModel()).ToArray());
    }

    public static WorkspacePermissionModel ToModel(this WorkspacePermission permission)
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
