using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Consent.Domain.Users;

namespace Consent.Domain.Workspaces;

/**
 *  A container for collaborating on contracts, permissions etc
 */

public class Workspace
{
    public WorkspaceId? Id { get; init; }

    private string _name;
    public string Name
    {
        get => _name;
        [MemberNotNull(nameof(_name))]
        private init
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(nameof(Name));
            }

            _name = value;
        }
    }

    private IImmutableList<Membership> _memberships;
    public IImmutableList<Membership> Memberships
    {
        get => _memberships;
        [MemberNotNull(nameof(_memberships))]
        private set
        {
            var users = value.Select(m => m.UserId);
            if (users.Count() != users.Distinct().Count())
            {
                throw new ArgumentException("Cannot have more than one membership for a user", nameof(Memberships));
            }

            if (!value.Any(m => m.IsSuperUser))
            {
                throw new ArgumentException("Workspace must have at least one superuser", nameof(Memberships));
            }

            _memberships = value;
        }
    }

    public Workspace(string name, IEnumerable<Membership> memberships)
    {
        Name = name;
        Memberships = memberships.ToImmutableList();
    }

    public Workspace(string name, UserId creator) :
        this(name, new[] { new Membership(creator, Membership.SuperUser) })
    {
    }

    private Workspace(string name) : this(name, Array.Empty<Membership>())
    {
    }

    public IEnumerable<WorkspacePermission> GetUserPermissions(UserId userId)
    {
        return Memberships.SingleOrDefault(m => m.UserId == userId)?.Permissions
            ?? Enumerable.Empty<WorkspacePermission>();
    }
}

public readonly record struct WorkspaceId(int Value) : IIdentifier;
