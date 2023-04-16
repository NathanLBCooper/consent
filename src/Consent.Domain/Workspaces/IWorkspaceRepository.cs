using System.Threading.Tasks;

namespace Consent.Domain.Workspaces;

public interface IWorkspaceRepository
{
    Task<Workspace?> Get(WorkspaceId id);
    Task<Workspace> Create(Workspace workspace);
    Task Update(Workspace workspace);
}
