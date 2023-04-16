using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Consent.Domain.Users;

/**
 *  A user of the system who manages and sets up things
 */

public class User
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
    public IReadOnlyCollection<WorkspaceMembership> WorkspaceMemberships => _workspaceMemberships.AsReadOnly();

    public User(string name, List<WorkspaceMembership> workspaceMemberships)
    {
        Name = name;
        _workspaceMemberships = workspaceMemberships;
    }

    public User(string name) : this(name, new())
    {
    }
}

public readonly record struct UserId(int Value) : IIdentifier;
