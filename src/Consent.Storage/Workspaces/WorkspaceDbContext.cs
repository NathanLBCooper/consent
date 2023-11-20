using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;
using Consent.Domain.Workspaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

        ConfigureWorkspaces(modelBuilder.Entity<Workspace>());
    }

    private void ConfigureWorkspaces(EntityTypeBuilder<Workspace> builder)
    {
        _ = builder.HasKey(e => e.Id);
        _ = builder.Property(e => e.Id).ValueGeneratedOnAdd();

        _ = builder.OwnsMany(e => e.Memberships, ConfigureWorkspaceMemberships);
    }

    private void ConfigureWorkspaceMemberships(OwnedNavigationBuilder<Workspace, Membership> builder)
    {
        _ = builder.UsePropertyAccessMode(PropertyAccessMode.Field);
        _ = builder.HasKey(e => e.Id);
        _ = builder.Property(e => e.Id).ValueGeneratedOnAdd();

        var enumToStr = new JsonSerializerOptions() { Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } };
        _ = builder.Property(e => e.Permissions)
            .HasConversion(
                v => JsonSerializer.Serialize(v, enumToStr),
                v => JsonSerializer.Deserialize<ImmutableList<Permission>>(v, enumToStr)!
                );
    }
}
