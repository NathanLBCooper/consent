using System;
using System.Linq;
using Consent.Api.Client.Models.Workspaces;
using Consent.Domain.Core;
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

    public static PermissionModel ToModel(this Permission permission)
    {
        return permission switch
        {
            Permission.View => PermissionModel.View,
            Permission.Edit => PermissionModel.Edit,
            Permission.Admin => PermissionModel.Admin,
            Permission.Buyer => PermissionModel.Buyer,
            _ => throw new ArgumentException()
        };
    }
}
