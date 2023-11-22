using Consent.Domain.Purposes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Consent.Storage.Purposes;

public class PurposeDbContext : DbContext
{
    public DbSet<Purpose> Purposes => Set<Purpose>();

    public PurposeDbContext(DbContextOptions<PurposeDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        TypeConverters.Configure(configurationBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.HasDefaultSchema("purposes");

        ConfigurePurposes(modelBuilder.Entity<Purpose>());
    }

    private void ConfigurePurposes(EntityTypeBuilder<Purpose> builder)
    {
        _ = builder.HasKey(e => e.Id);
        _ = builder.Property(e => e.Id).ValueGeneratedOnAdd();
    }
}
