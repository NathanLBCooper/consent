using System.Threading.Tasks;
using Consent.Domain.Workspaces;
using Microsoft.EntityFrameworkCore;

namespace Consent.Storage.Workspaces;

public class WorkspaceRepository : IWorkspaceRepository
{
    private readonly WorkspaceDbContext _dbContext;

    public WorkspaceRepository(WorkspaceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Workspace?> Get(WorkspaceId id)
    {
        if (await _dbContext.Workspaces.FindAsync(id) is Workspace workspace)
        {
            return workspace;
        }

        return null;
    }

    public async Task<Workspace> Create(Workspace workspace)
    {
        _ = await _dbContext.AddAsync(workspace);
        _ = await _dbContext.SaveChangesAsync();

        return workspace;
    }

    public async Task Update(Workspace workspace)
    {
        _dbContext.Attach(workspace).State = EntityState.Modified;
        _ = await _dbContext.SaveChangesAsync();
    }
}
