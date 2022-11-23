﻿using System;

namespace Consent.Domain.Workspaces
{
    /**
     *  A container for ... something todo
     */

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
}
