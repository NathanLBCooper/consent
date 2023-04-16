using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Consent.Domain.Users;
using Consent.Domain.Workspaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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

        _ = modelBuilder.Entity<User>(eb =>
        {
            _ = eb.HasKey(e => e.Id);
            _ = eb.Property(e => e.Id).ValueGeneratedOnAdd();

            eb.Metadata.FindNavigation(nameof(User.WorkspaceMemberships))!.SetPropertyAccessMode(PropertyAccessMode.Field);
            _ = eb.HasMany(e => e.WorkspaceMemberships).WithOne();
            _ = eb.Navigation(e => e.WorkspaceMemberships).AutoInclude();
        });

        _ = modelBuilder.Entity<WorkspaceMembership>(eb =>
        {
            _ = eb.ToView("WorkspaceMembership");
            _ = eb.HasOne<User>().WithMany(e => e.WorkspaceMemberships);
        });

        var enumToStr = new JsonSerializerOptions() { Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } };

        _ = modelBuilder.Entity<WorkspaceMembership>()
            .Property(e => e.Permissions)
            .HasConversion(
                v => JsonSerializer.Serialize(v, enumToStr),
                v => JsonSerializer.Deserialize<List<WorkspacePermission>>(v, enumToStr)!,
                new ValueComparer<IReadOnlyCollection<WorkspacePermission>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToHashSet())
                );
    }
}
