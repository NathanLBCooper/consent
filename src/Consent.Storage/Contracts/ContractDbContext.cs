using System.Collections.Immutable;
using System.Text.Json;
using Consent.Domain.Contracts;
using Consent.Domain.Purposes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

        ConfigureContracts(modelBuilder.Entity<Contract>());
    }

    private void ConfigureContracts(EntityTypeBuilder<Contract> builder)
    {
        _ = builder.HasKey(e => e.Id);
        _ = builder.Property(e => e.Id).ValueGeneratedOnAdd();

        _ = builder.OwnsMany(e => e.Versions, ConfigureVersions);
    }

    private void ConfigureVersions(OwnedNavigationBuilder<Contract, ContractVersion> builder)
    {
        _ = builder.UsePropertyAccessMode(PropertyAccessMode.Field);
        _ = builder.HasKey(e => e.Id);
        _ = builder.Property(e => e.Id).ValueGeneratedOnAdd();

        _ = builder.OwnsMany(e => e.Provisions, ConfigureProvisions);
    }

    private void ConfigureProvisions(OwnedNavigationBuilder<ContractVersion, Provision> builder)
    {
        _ = builder.UsePropertyAccessMode(PropertyAccessMode.Field);
        _ = builder.HasKey(e => e.Id);
        _ = builder.Property(e => e.Id).ValueGeneratedOnAdd();

        _ = builder.Property(e => e.PurposeIds)
           .HasConversion(
               v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
               v => JsonSerializer.Deserialize<ImmutableList<PurposeId>>(v, (JsonSerializerOptions)null!)!
               );
    }
}
