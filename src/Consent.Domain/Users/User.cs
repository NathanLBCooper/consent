using System;
using System.Collections.Generic;
using Consent.Domain.Workspaces;

namespace Consent.Domain.Users;

/**
 *  A user of the system who manages and sets up things
 */

public class User
{
    public UserId? Id { get; init; }

    public string Name { get; private set; }
    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(Name));
        }
    }

    private readonly List<WorkspaceMembership> _workspaceMemberships;
    public IReadOnlyCollection<WorkspaceMembership> WorkspaceMemberships => _workspaceMemberships.AsReadOnly();

    public User(string name)
    {
        ValidateName(name);
        Name = name;

        _workspaceMemberships = new List<WorkspaceMembership>();
    }
}

public readonly record struct UserId(int Value) : IIdentifier;

public class WorkspaceMembership
{
    public MembershipId Id { get; init; }
    public WorkspaceId WorkspaceId { get; init; }

    private readonly List<WorkspacePermission> _permissions = new();
    public IReadOnlyCollection<WorkspacePermission> Permissions => _permissions.AsReadOnly();
}
