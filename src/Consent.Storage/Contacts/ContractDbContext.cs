using Consent.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Consent.Storage.Contacts;

public class ContractDbContext : DbContext
{
    public ContractDbContext(DbContextOptions<ContractDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public DbSet<Contract> Contracts => Set<Contract>();
}
