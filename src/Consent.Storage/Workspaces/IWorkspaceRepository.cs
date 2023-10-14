using System.Threading.Tasks;
using Consent.Domain.Workspaces;

namespace Consent.Storage.Workspaces;

public interface IWorkspaceRepository
{
    Task<Workspace?> Get(WorkspaceId id);
    Task<Workspace> Create(Workspace workspace);
    Task Update(Workspace workspace);
}
