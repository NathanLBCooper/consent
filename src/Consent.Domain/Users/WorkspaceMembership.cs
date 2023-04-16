using System.Collections.Generic;
using Consent.Domain.Workspaces;

namespace Consent.Domain.Users;

public class WorkspaceMembership
{
    public MembershipId Id { get; private init; }
    public WorkspaceId WorkspaceId { get; private init; }

    private readonly List<WorkspacePermission> _permissions = new();
    public IReadOnlyCollection<WorkspacePermission> Permissions => _permissions.AsReadOnly();

    public WorkspaceMembership(MembershipId id, WorkspaceId workspaceId, List<WorkspacePermission> permissions)
    {
        Id = id;
        WorkspaceId = workspaceId;
        _permissions = permissions;
    }

    private WorkspaceMembership() : this(default, default, new())
    {
    }
}
