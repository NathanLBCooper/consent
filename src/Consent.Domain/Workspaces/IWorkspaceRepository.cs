﻿using Consent.Domain.Users;
using System.Threading.Tasks;

namespace Consent.Domain.Workspaces
{
    public interface IWorkspaceRepository
    {
        Task<WorkspaceEntity?> Get(WorkspaceId id);
        Task<WorkspaceEntity> Create(Workspace workspace);
        Task<WorkspacePermission[]> PermissionsGet(UserId userId, WorkspaceId workspaceId);
        Task<WorkspaceMembership[]> MembershipsGet(WorkspaceId workspaceId);
        Task PermissionsCreate(UserId userId, WorkspaceId workspaceId, WorkspacePermission[] permissions);
    }
}