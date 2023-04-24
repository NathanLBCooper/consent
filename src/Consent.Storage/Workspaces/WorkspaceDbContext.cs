using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;
using Consent.Domain.Workspaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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

        modelBuilder.Entity<Workspace>().Metadata.FindNavigation(nameof(Workspace.Memberships))
            !.SetPropertyAccessMode(PropertyAccessMode.Field);

        _ = modelBuilder.Entity<Membership>().HasKey(e => e.Id);
        _ = modelBuilder.Entity<Membership>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        var enumToStr = new JsonSerializerOptions() { Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } };

        _ = modelBuilder.Entity<Membership>()
            .Property(e => e.Permissions)
            .HasConversion(
                v => JsonSerializer.Serialize(v, enumToStr),
                v => JsonSerializer.Deserialize<ImmutableList<WorkspacePermission>>(v, enumToStr)!
                );
    }
}
