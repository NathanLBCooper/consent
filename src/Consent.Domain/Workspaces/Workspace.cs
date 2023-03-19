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

    public string Name { get; private set; }
    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(Name));
        }
    }

    private readonly List<Membership> _memberships;
    public IReadOnlyCollection<Membership> Memberships => _memberships;

    private static void ValidateMemberships(List<Membership> memberships)
    {
        var users = memberships.Select(m => m.User);
        if (users.Count() != users.Distinct().Count())
        {
            throw new ArgumentException("Cannot have more than one membership for a user", nameof(Memberships));
        }

        if (!memberships.Any(m => m.IsSuperUser))
        {
            throw new ArgumentException("Workspace must have at least one superuser", nameof(Memberships));
        }
    }

    public Workspace(string name, List<Membership> memberships)
    {
        ValidateName(name);
        Name = name;

        ValidateMemberships(memberships);
        _memberships = memberships;
    }

    public Workspace(string name, UserId creator)
    {
        ValidateName(name);
        Name = name;

        _memberships = new List<Membership> { new Membership(creator, Membership.SuperUser.ToList()) };
    }

    // todo for EF, compromises validity
    protected Workspace(string name)
    {
        ValidateName(name);
        Name = name;

        // todo invalid
        _memberships = new List<Membership>();
    }

    public IEnumerable<WorkspacePermission> GetUserPermissions(UserId user)
    {
        return Memberships.SingleOrDefault(m => m.User == user)?.Permissions
            ?? Enumerable.Empty<WorkspacePermission>();
    }
}

public readonly record struct WorkspaceId(int Value) : IIdentifier;
