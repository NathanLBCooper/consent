using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Consent.Domain.Workspaces;

public static class WorkspacePermissionsExtensions
{
    public static bool CanView(this IEnumerable<WorkspacePermission> permissions)
    {
        return permissions.Contains(WorkspacePermission.View);
    }

    public static bool CanEdit(this IEnumerable<WorkspacePermission> permissions)
    {
        return permissions.Contains(WorkspacePermission.Edit);
    }
}
