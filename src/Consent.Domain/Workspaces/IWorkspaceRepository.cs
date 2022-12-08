using System.Threading.Tasks;

namespace Consent.Domain.Workspaces;

public interface IWorkspaceRepository
{
    Task<WorkspaceEntity?> Get(WorkspaceId id);
    Task<WorkspaceEntity> Create(Workspace workspace);
    Task Update(WorkspaceEntity workspace);
}
