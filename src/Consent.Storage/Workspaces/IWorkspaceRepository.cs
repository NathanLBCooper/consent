using Consent.Domain.Workspaces;

namespace Consent.Storage.Workspaces;

public interface IWorkspaceRepository : IRepository<Workspace, WorkspaceId>
{
}
