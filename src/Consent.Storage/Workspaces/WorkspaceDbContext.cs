using Consent.Domain.Workspaces;
using Microsoft.EntityFrameworkCore;

namespace Consent.Storage.Workspaces;

public class WorkspaceDbContext : DbContext
{
    public DbSet<Workspace> Workspaces => Set<Workspace>();

    public WorkspaceDbContext(DbContextOptions<WorkspaceDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        TypeConverters.Configure(configurationBuilder);
    }
}
