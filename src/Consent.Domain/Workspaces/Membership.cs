using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Consent.Domain.Core;
using Consent.Domain.Users;

namespace Consent.Domain.Workspaces;

public class Membership
{
    public static ImmutableArray<WorkspacePermission> SuperUser { get; } = ImmutableArray.Create(
        WorkspacePermission.View,
        WorkspacePermission.Edit,
        WorkspacePermission.Admin,
        WorkspacePermission.Buyer
        );

    public MembershipId Id { get; init; }
    public UserId UserId { get; private init; }
    public ImmutableList<WorkspacePermission> Permissions { get; private init; }

    public bool IsSuperUser { get; }
    public bool CanView => Permissions.Contains(WorkspacePermission.View);
    public bool CanEdit => Permissions.Contains(WorkspacePermission.Edit);

    public Membership(UserId userId, IEnumerable<WorkspacePermission> permissions)
    {
        UserId = userId;
        Permissions = permissions.ToImmutableList();

        IsSuperUser = SuperUser.All(Permissions.Contains);
    }

    private Membership(UserId userId) : this(userId, Array.Empty<WorkspacePermission>())
    {
    }
}

public readonly record struct MembershipId(int Value) : IIdentifier;
