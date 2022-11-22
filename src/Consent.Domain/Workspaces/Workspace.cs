using Consent.Domain.Users;
using System;

namespace Consent.Domain.Workspaces
{
    public record Workspace
    {
        public string Name { get; private init; }

        public Workspace(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(Name));
            Name = name;
        }
    }

    public record WorkspaceEntity : Workspace, IEntity
    {
        public int Id { get; private init; }

        public WorkspaceEntity(int id, Workspace workspace) : base(workspace)
        {
            Id = id;
        }

        public WorkspaceEntity(int id, string name) : base(name)
        {
            Id = id;
        }
    }

    public record WorkspaceMembership
    {
        public int UserId { get; private init; }
        public int WorkspaceId { get; private init; }
        public WorkspacePermission[] Permissions { get; private init; }

        public WorkspaceMembership(int userId, int workspaceId, WorkspacePermission[] permissions)
        {
            UserId = userId;
            WorkspaceId = workspaceId;
            Permissions = permissions;
        }
    }

    public enum WorkspacePermission
    {
        View,
        Edit,
        Admin,
        Buyer
    }
}
