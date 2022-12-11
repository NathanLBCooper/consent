using System;

namespace Consent.Domain.Workspaces.Permissions;

/*
 *  A specific idea that can be agreed to
 */

public class Permission
{
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

public record struct PermissionId(int Value);

public class PermissionEntity : Permission
{
    public PermissionId Id { get; private init; }

    public PermissionEntity(PermissionId id, WorkspaceId workspaceId, string name, string description)
        : base(workspaceId, name, description)
    {
        Id = id;
    }
}
