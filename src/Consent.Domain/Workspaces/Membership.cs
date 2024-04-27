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

    public static Membership Ctor(UserId userId, IEnumerable<Permission> permissions)
    {
        var p = permissions.ToImmutableList();
        var isSuperUser = SuperUser.All(p.Contains);

        return new Membership(userId, p, isSuperUser);
    }

    private Membership(UserId userId, ImmutableList<Permission> permissions, bool isSuperUser)
    {
        UserId = userId;
        Permissions = permissions;
        IsSuperUser = isSuperUser;
    }

    private Membership(UserId userId) : this(userId, ImmutableList<Permission>.Empty, false)
    {
    }
}

public readonly record struct MembershipId(int Value) : IIdentifier;
