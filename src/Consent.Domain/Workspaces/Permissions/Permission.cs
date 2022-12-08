using System;

namespace Consent.Domain.Workspaces.Permissions;

/*
 *  A specific idea that can be agreed to
 */

public record Permission
{
    public WorkspaceId WorkspaceId { get; private init; }
    public string Name { get; private init; }
    public string Description { get; private init; }

    public Permission(WorkspaceId workspaceId, string name, string description)
    {
        WorkspaceId = workspaceId;

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(Name));
        }

        Name = name;

        Description = description;
    }
}

public record struct PermissionId(int Value);

public record PermissionEntity : Permission
{
    public PermissionId Id { get; private init; }

    public PermissionEntity(PermissionId id, Permission permission) : base(permission)
    {
        Id = id;
    }

    public PermissionEntity(PermissionId id, WorkspaceId workspaceId, string name, string description) : base(workspaceId, name, description)
    {
        Id = id;
    }
}
