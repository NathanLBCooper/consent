using System.Collections.Generic;
using Consent.Domain.Workspaces;

namespace Consent.Domain.Users;

public class WorkspaceMembership
{
    public MembershipId Id { get; private init; }
    public WorkspaceId WorkspaceId { get; private init; }
    public IReadOnlyCollection<WorkspacePermission> Permissions { get; private init; }

    public WorkspaceMembership(MembershipId id, WorkspaceId workspaceId, List<WorkspacePermission> permissions)
    {
        Id = id;
        WorkspaceId = workspaceId;
        Permissions = permissions.AsReadOnly();
    }

    private WorkspaceMembership() : this(default, default, new())
    {
    }
}
