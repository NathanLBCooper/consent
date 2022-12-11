using System;
using System.Collections.Generic;
using System.Linq;
using Consent.Domain.Users;

namespace Consent.Domain.Workspaces;

/**
 *  A container for collaborating on contracts, permissions etc
 */

public class Workspace
{
    public string Name { get; }
    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(Name));
        }
    }

    public WorkspaceMembership[] Memberships;
    private static void ValidateMemberships(WorkspaceMembership[] memberships)
    {
        var users = memberships.Select(m => m.UserId);
        if (users.Count() != users.Distinct().Count())
        {
            throw new ArgumentException("Cannot have more than one membership for a user", nameof(Memberships));
        }

        if (!memberships.Any(m => m.IsSuperUser()))
        {
            throw new ArgumentException("Workspace must have at least one superuser", nameof(Memberships));
        }
    }

    public Workspace(string name, WorkspaceMembership[] memberships)
    {
        ValidateName(name);
        ValidateMemberships(memberships);

        Name = name;
        Memberships = memberships;
    }

    public Workspace(string name, UserId creatorId)
    {
        ValidateName(name);

        Name = name;
        Memberships = new[] { new WorkspaceMembership(creatorId, WorkspaceMembership.SuperUser) };
    }

    public IEnumerable<WorkspacePermission> GetUserPermissions(UserId userId)
    {
        return Memberships.SingleOrDefault(m => m.UserId == userId)?.Permissions
            ?? Enumerable.Empty<WorkspacePermission>();
    }
}

public record struct WorkspaceId(int Value);

public class WorkspaceEntity : Workspace
{
    public WorkspaceId Id { get; }

    public WorkspaceEntity(WorkspaceId id, string name, WorkspaceMembership[] memberships)
        : base(name, memberships)
    {
        Id = id;
    }
}
