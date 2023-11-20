using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;
using Consent.Domain.Users;
using Consent.Domain.Workspaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Consent.Storage.Users;

public class UserDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        TypeConverters.Configure(configurationBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.HasDefaultSchema("users");

        ConfigureUsers(modelBuilder.Entity<User>());
    }

    private void ConfigureUsers(EntityTypeBuilder<User> builder)
    {
        _ = builder.HasKey(e => e.Id);
        _ = builder.Property(e => e.Id).ValueGeneratedOnAdd();

        _ = builder.OwnsMany(e => e.WorkspaceMemberships, ConfigureWorkspaceMemeberships);
    }

    private void ConfigureWorkspaceMemeberships(OwnedNavigationBuilder<User, WorkspaceMembership> builder)
    {
        _ = builder.UsePropertyAccessMode(PropertyAccessMode.Field);
        _ = builder.ToView("WorkspaceMembership");

        var enumToStr = new JsonSerializerOptions() { Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } };
        _ = builder.Property(e => e.Permissions)
            .HasConversion(
                v => JsonSerializer.Serialize(v, enumToStr),
                v => JsonSerializer.Deserialize<ImmutableList<Permission>>(v, enumToStr)!
                );
    }
}
