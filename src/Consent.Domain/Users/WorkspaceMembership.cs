using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Consent.Domain.Workspaces;

namespace Consent.Domain.Users;

public class WorkspaceMembership
{
    public MembershipId Id { get; private init; }
    public WorkspaceId WorkspaceId { get; private init; }
    public ImmutableList<WorkspacePermission> Permissions { get; private init; }

    public WorkspaceMembership(MembershipId id, WorkspaceId workspaceId, IEnumerable<WorkspacePermission> permissions)
    {
        Id = id;
        WorkspaceId = workspaceId;
        Permissions = permissions.ToImmutableList();
    }

    private WorkspaceMembership() : this(default, default, Array.Empty<WorkspacePermission>())
    {
    }
}
