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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.HasDefaultSchema("workspaces");

        _ = modelBuilder.Entity<Workspace>().HasKey(e => e.Id);
        _ = modelBuilder.Entity<Workspace>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        _ = modelBuilder.Entity<Membership>().HasKey(e => e.Id);
        _ = modelBuilder.Entity<Membership>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();
    }
}
