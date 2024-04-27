using System;
using System.Collections.Generic;
using System.Linq;
using Consent.Domain.Core;
using Consent.Domain.Core.Errors;
using Consent.Domain.Users;

namespace Consent.Domain.Workspaces;

/**
 *  A container for collaborating on contracts, purposes etc
 */
public class Workspace : IEntity<WorkspaceId>
{
    public WorkspaceId? Id { get; init; }

    public string Name { get; init; }
    private static Result ValidateName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure(new ArgumentError(nameof(Name)));
        }

        return Result.Success();
    }

    private readonly List<Membership> _memberships;
    public IReadOnlyList<Membership> Memberships => _memberships.AsReadOnly();

    private static Result ValidateMembership(List<Membership> value)
    {
        var users = value.Select(m => m.UserId);
        var hasDuplicates = users.GroupBy(u => u).Any(g => g.Count() > 1);
        if (hasDuplicates)
        {
            return Result.Failure(new ArgumentError(nameof(Memberships),
                "Cannot have more than one membership for a user"));
        }

        if (!value.Any(m => m.IsSuperUser))
        {
            return Result.Failure(new ArgumentError(nameof(Memberships), "Workspace must have at least one superuser"));
        }

        return Result.Success();
    }

    public static Result<Workspace> Ctor(string name, IEnumerable<Membership> memberships)
    {
        if (ValidateName(name) is { IsSuccess: false } nameResult)
        {
            return Result<Workspace>.Failure(nameResult.Error);
        }

        var m = memberships.ToList();
        if (ValidateMembership(m) is { IsSuccess: false } mResult)
        {
            return Result<Workspace>.Failure(mResult.Error);
        }

        return Result<Workspace>.Success(new Workspace(name, m));
    }

    public static Result<Workspace> Ctor(string name, UserId creator)
    {
        var creatorAsSuperUser = Membership.Ctor(creator, Membership.SuperUser);
        return Ctor(name, new[] { creatorAsSuperUser });
    }

    private Workspace(string name, IEnumerable<Membership> memberships)
    {
        Name = name;
        _memberships = memberships.ToList();
    }

    private Workspace(string name) : this(name, Array.Empty<Membership>())
    {
    }

    public bool UserCanView(UserId userId)
    {
        return Memberships.SingleOrDefault(m => m.UserId == userId)?.CanView ?? false;
    }

    public bool UserCanEdit(UserId userId)
    {
        return Memberships.SingleOrDefault(m => m.UserId == userId)?.CanEdit ?? false;
    }

    public IEnumerable<Permission> GetUserPermissions(UserId userId)
    {
        return Memberships.SingleOrDefault(m => m.UserId == userId)?.Permissions
               ?? Enumerable.Empty<Permission>();
    }
}

public readonly record struct WorkspaceId(int Value) : IIdentifier;
