using System;
using System.Collections.Generic;
using System.Linq;
using Consent.Domain.Core;
using Consent.Domain.Core.Errors;

namespace Consent.Domain.Users;

/**
 *  A user of the system who manages and sets up things
 */
public class User : IEntity<UserId>
{
    public UserId? Id { get; init; }

    public string Name { get; private init; }

    private readonly List<WorkspaceMembership> _workspaceMemberships;
    public IReadOnlyList<WorkspaceMembership> WorkspaceMemberships => _workspaceMemberships.AsReadOnly();

    private User(string name, IEnumerable<WorkspaceMembership> workspaceMemberships)
    {
        Name = name;
        _workspaceMemberships = workspaceMemberships.ToList();
    }

    private User(string name) : this(name, Array.Empty<WorkspaceMembership>())
    {
    }

    public static Result<User> Ctor(string name, IEnumerable<WorkspaceMembership> workspaceMemberships)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<User>.Failure(new ArgumentError(nameof(Name)));
        }

        return Result<User>.Success(new User(name, workspaceMemberships));
    }

    public static Result<User> Ctor(string name)
    {
        return Ctor(name, Array.Empty<WorkspaceMembership>());
    }
}

public readonly record struct UserId(int Value) : IIdentifier;
