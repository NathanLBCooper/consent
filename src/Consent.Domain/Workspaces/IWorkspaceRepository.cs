using System.Threading.Tasks;

namespace Consent.Domain.Workspaces
{
    public interface IWorkspaceRepository
    {
        Task<WorkspaceEntity?> Get(int id);
        Task<WorkspaceEntity> Create(Workspace workspace);
        Task<WorkspacePermission[]> PermissionsGet(int userId, int workspaceId);
        Task<WorkspaceMembership[]> MembershipsGet(int workspaceId);
        Task PermissionsCreate(int userId, int workspaceId, WorkspacePermission[] permissions);
    }
}
