using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Consent.Domain.Core;
using Consent.Domain.Workspaces;

namespace Consent.Domain.Users;

/**
 *  A user of the system who manages and sets up things
 */

public class User : IEntity<UserId>
{
    public UserId? Id { get; init; }

    private readonly string _name;
    public string Name
    {
        get => _name;
        [MemberNotNull(nameof(_name))]
        init
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _name = value;
                throw new ArgumentException(nameof(Name));
            }

            _name = value;
        }
    }

    private readonly List<WorkspaceMembership> _workspaceMemberships;
    public IReadOnlyList<WorkspaceMembership> WorkspaceMemberships => _workspaceMemberships.AsReadOnly();

    public User(string name, IEnumerable<WorkspaceMembership> workspaceMemberships)
    {
        Name = name;
        _workspaceMemberships = workspaceMemberships.ToList();
    }

    public User(string name) : this(name, Array.Empty<WorkspaceMembership>())
    {
    }

    public bool CanViewWorkspace(WorkspaceId workspaceId)
    {
        return WorkspaceMemberships.SingleOrDefault(m => m.WorkspaceId == workspaceId)?.CanView ?? false;
    }
}

public readonly record struct UserId(int Value) : IIdentifier;
