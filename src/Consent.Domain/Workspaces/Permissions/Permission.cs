using System;

namespace Consent.Domain.Workspaces.Permissions;

/*
 *  A specific idea that can be agreed to
 */

public class Permission
{
    public WorkspaceId WorkspaceId { get; }

    public string Name { get; }
    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(Name));
        }
    }

    public string Description { get; }

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
    public PermissionId Id { get; }

    public PermissionEntity(PermissionId id, WorkspaceId workspaceId, string name, string description)
        : base(workspaceId, name, description)
    {
        Id = id;
    }
}
