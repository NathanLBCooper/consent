using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Consent.Domain.Core;
using Consent.Domain.Users;

namespace Consent.Domain.Workspaces;

public class Membership
{
    public static ImmutableArray<Permission> SuperUser { get; } =
        [Permission.View, Permission.Edit, Permission.Admin, Permission.Buyer];

    public MembershipId Id { get; init; }
    public UserId UserId { get; private init; }
    public ImmutableList<Permission> Permissions { get; private init; }

    public bool IsSuperUser { get; }
    public bool CanView => Permissions.Contains(Permission.View);
    public bool CanEdit => Permissions.Contains(Permission.Edit);

    public Membership(UserId userId, IEnumerable<Permission> permissions)
    {
        UserId = userId;
        Permissions = permissions.ToImmutableList();

        IsSuperUser = SuperUser.All(Permissions.Contains);
    }

    private Membership(UserId userId) : this(userId, Array.Empty<Permission>())
    {
    }
}

public readonly record struct MembershipId(int Value) : IIdentifier;
