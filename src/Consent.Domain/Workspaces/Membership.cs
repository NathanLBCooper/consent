using System;
using System.Collections.Generic;
using System.Linq;
using Consent.Domain.Users;

namespace Consent.Domain.Workspaces;

public class Membership
{
    public static IReadOnlyCollection<WorkspacePermission> SuperUser { get; } = new List<WorkspacePermission> {
        WorkspacePermission.View,
        WorkspacePermission.Edit,
        WorkspacePermission.Admin,
        WorkspacePermission.Buyer
    };

    public MembershipId Id { get; init; }
    public UserId User { get; private init; }

    private readonly List<WorkspacePermission> _permissions;
    public IReadOnlyCollection<WorkspacePermission> Permissions => _permissions.AsReadOnly();

    public bool IsSuperUser { get; }

    public Membership(UserId user, List<WorkspacePermission> permissions)
    {
        User = user;
        _permissions = permissions;

        IsSuperUser = SuperUser.All(p => Permissions.Contains(p));
    }

    public Membership(UserId user) : this(user, new List<WorkspacePermission>())
    {
    }
}

public readonly record struct MembershipId(int Value) : IIdentifier;
