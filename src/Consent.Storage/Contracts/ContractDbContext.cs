using System.Collections.Immutable;
using System.Text.Json;
using Consent.Domain.Contracts;
using Consent.Domain.Permissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Consent.Storage.Contracts;

public class ContractDbContext : DbContext
{
    public DbSet<Contract> Contracts => Set<Contract>();

    public ContractDbContext(DbContextOptions<ContractDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        TypeConverters.Configure(configurationBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.HasDefaultSchema("contracts");

        _ = modelBuilder.Entity<Contract>().HasKey(e => e.Id);
        _ = modelBuilder.Entity<Contract>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        _ = modelBuilder.Entity<ContractVersion>().HasKey(e => e.Id);
        _ = modelBuilder.Entity<ContractVersion>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        _ = modelBuilder.Entity<Provision>().HasKey(e => e.Id);
        _ = modelBuilder.Entity<Provision>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        _ = modelBuilder.Entity<Provision>()
            .Property(e => e.PermissionIds)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
                v => JsonSerializer.Deserialize<ImmutableList<PermissionId>>(v, (JsonSerializerOptions)null!)!
                );
    }
}
