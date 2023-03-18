using Consent.Domain.Workspaces;
using Microsoft.EntityFrameworkCore;

namespace Consent.Storage.Workspaces;

public class WorkspaceDbContext : DbContext
{
    public WorkspaceDbContext(DbContextOptions<WorkspaceDbContext> options) : base(options)
    {
    }

    public DbSet<Workspace> Workspaces => Set<Workspace>();
}
