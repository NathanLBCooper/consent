using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Consent.Domain.Core;
using Consent.Domain.Users;

namespace Consent.Domain.Workspaces;

/**
 *  A container for collaborating on contracts, permissions etc
 */

public class Workspace : IEntity<WorkspaceId>
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

    private List<Membership> _memberships;
    public IReadOnlyList<Membership> Memberships => _memberships.AsReadOnly();
    [MemberNotNull(nameof(_memberships))]
    private void SetMemberships(List<Membership> value)
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

        _memberships = value.ToList();
    }

    public Workspace(string name, IEnumerable<Membership> memberships)
    {
        Name = name;
        SetMemberships(memberships.ToList());
    }

    public Workspace(string name, UserId creator) :
        this(name, new[] { new Membership(creator, Membership.SuperUser) })
    {
    }

    private Workspace(string name)
    {
        Name = name;
        _memberships = new List<Membership>();
    }

    public IEnumerable<WorkspacePermission> GetUserPermissions(UserId userId)
    {
        return Memberships.SingleOrDefault(m => m.UserId == userId)?.Permissions
            ?? Enumerable.Empty<WorkspacePermission>();
    }
}

public readonly record struct WorkspaceId(int Value) : IIdentifier;
