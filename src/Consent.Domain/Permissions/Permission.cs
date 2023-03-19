using System;
using Consent.Domain.Workspaces;

namespace Consent.Domain.Permissions;

/*
 *  A specific idea that can be agreed to
 */

public class Permission
{
    public PermissionId? Id { get; init; }
    public WorkspaceId WorkspaceId { get; private init; }

    public string Name { get; private init; }
    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(Name));
        }
    }

    public string Description { get; private init; }

    public Permission(WorkspaceId workspaceId, string name, string description)
    {
        WorkspaceId = workspaceId;

        ValidateName(name);
        Name = name;

        Description = description;
    }
}

public readonly record struct PermissionId(int Value) : IIdentifier;
