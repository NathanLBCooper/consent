using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Consent.Domain.Contracts;
using Consent.Domain.Permissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Consent.Storage.Contacts;

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

        modelBuilder.Entity<Contract>().Metadata.FindNavigation(nameof(Contract.Versions))
            !.SetPropertyAccessMode(PropertyAccessMode.Field);

        _ = modelBuilder.Entity<ContractVersion>().HasKey(e => e.Id);
        _ = modelBuilder.Entity<ContractVersion>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<ContractVersion>().Metadata.FindNavigation(nameof(ContractVersion.Provisions))
            !.SetPropertyAccessMode(PropertyAccessMode.Field);

        _ = modelBuilder.Entity<Provision>().HasKey(e => e.Id);
        _ = modelBuilder.Entity<Provision>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        _ = modelBuilder.Entity<Provision>()
            .Property(e => e.Permissions)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
                v => JsonSerializer.Deserialize<List<PermissionId>>(v, (JsonSerializerOptions)null!)!,
                new ValueComparer<IReadOnlyCollection<PermissionId>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToHashSet())
                );
    }
}
