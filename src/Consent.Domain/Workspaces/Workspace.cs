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
    public WorkspaceId? Id { get; init; }

    public string Name { get; }
    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(Name));
        }
    }

    public Membership[] Memberships;
    private static void ValidateMemberships(Membership[] memberships)
    {
        var users = memberships.Select(m => m.UserId);
        if (users.Count() != users.Distinct().Count())
        {
            throw new ArgumentException("Cannot have more than one membership for a user", nameof(Memberships));
        }

        if (!memberships.Any(m => m.IsSuperUser))
        {
            throw new ArgumentException("Workspace must have at least one superuser", nameof(Memberships));
        }
    }

    public Workspace(string name, Membership[] memberships)
    {
        ValidateName(name);
        Name = name;

        ValidateMemberships(memberships);
        Memberships = memberships;
    }

    public Workspace(string name, UserId creatorId)
    {
        ValidateName(name);
        Name = name;

        Memberships = new[] { new Membership(creatorId, Membership.SuperUser) };
    }

    public IEnumerable<WorkspacePermission> GetUserPermissions(UserId userId)
    {
        return Memberships.SingleOrDefault(m => m.UserId == userId)?.Permissions
            ?? Enumerable.Empty<WorkspacePermission>();
    }
}

public record struct WorkspaceId(int Value);
