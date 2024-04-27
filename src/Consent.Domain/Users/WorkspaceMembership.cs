using System.Collections.Generic;
using System.Collections.Immutable;
using Consent.Domain.Workspaces;

namespace Consent.Domain.Users;

public class WorkspaceMembership
{
    public MembershipId Id { get; private init; }
    public WorkspaceId WorkspaceId { get; private init; }
    public ImmutableList<Permission> Permissions { get; private init; }

    public static WorkspaceMembership Ctor(MembershipId id, WorkspaceId workspaceId,
        IEnumerable<Permission> permissions)
    {
        return new WorkspaceMembership(id, workspaceId, permissions.ToImmutableList());
    }

    private WorkspaceMembership(MembershipId id, WorkspaceId workspaceId, ImmutableList<Permission> permissions)
    {
        Id = id;
        WorkspaceId = workspaceId;
        Permissions = permissions;
    }

    private WorkspaceMembership() : this(default, default, ImmutableList<Permission>.Empty)
    {
    }
}
