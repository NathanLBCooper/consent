using Consent.Application.Workspaces;
using Consent.Domain.Workspaces;

namespace Consent.Storage.Workspaces;

public class WorkspaceRepository : Repository<Workspace, WorkspaceId>, IWorkspaceRepository
{
    public WorkspaceRepository(WorkspaceDbContext dbContext) : base(dbContext)
    {
    }
}
