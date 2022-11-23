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

    public record WorkspaceEntity : Workspace
    {
        public WorkspaceId Id { get; private init; }

        public WorkspaceEntity(WorkspaceId id, Workspace workspace) : base(workspace)
        {
            Id = id;
        }

        public WorkspaceEntity(WorkspaceId id, string name) : base(name)
        {
            Id = id;
        }
    }

    public record struct WorkspaceId(int Value);

    public record WorkspaceMembership
    {
        public UserId UserId { get; private init; }
        public WorkspaceId WorkspaceId { get; private init; }
        public WorkspacePermission[] Permissions { get; private init; }

        public WorkspaceMembership(UserId userId, WorkspaceId workspaceId, WorkspacePermission[] permissions)
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
