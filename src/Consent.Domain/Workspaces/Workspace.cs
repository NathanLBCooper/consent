using System;
using System.Collections.Generic;
using System.Linq;
using Consent.Domain.Users;

namespace Consent.Domain.Workspaces;

/**
 *  A container for ... something todo
 */

public record Workspace
{
    public string Name { get; }
    public WorkspaceMembership[] Memberships;

    public Workspace(string name, WorkspaceMembership[] memberships)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(Name));
        }

        Name = name;
        Memberships = memberships;
    }

    public IEnumerable<WorkspacePermission> GetUserPermissions(UserId userId)
    {
        return Memberships.SingleOrDefault(m => m.UserId == userId)?.Permissions ?? Enumerable.Empty<WorkspacePermission>();
    }
}

public record struct WorkspaceId(int Value);


public record WorkspaceEntity : Workspace
{
    public WorkspaceId Id { get; private init; }

    public WorkspaceEntity(WorkspaceId id, Workspace participant) : base(participant)
    {
        Id = id;
    }
}
