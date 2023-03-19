using Consent.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

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
    }
}
