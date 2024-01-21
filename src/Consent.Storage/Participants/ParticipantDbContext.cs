using Consent.Domain.Participants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Consent.Storage.Participants;

public class ParticipantDbContext : DbContext
{
    public DbSet<Participant> Participants => Set<Participant>();
    public DbSet<Tag> Tags => Set<Tag>();

    public ParticipantDbContext(DbContextOptions<ParticipantDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        TypeConverters.Configure(configurationBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.HasDefaultSchema("participants");

        ConfigureParticipants(modelBuilder.Entity<Participant>());
        ConfigureTags(modelBuilder.Entity<Tag>());
    }

    private void ConfigureParticipants(EntityTypeBuilder<Participant> builder)
    {
        _ = builder.HasKey(e => e.Id);
        _ = builder.Property(e => e.Id).ValueGeneratedOnAdd();
    }

    private void ConfigureTags(EntityTypeBuilder<Tag> builder)
    {
        _ = builder.HasKey(e => e.Id);
        _ = builder.Property(e => e.Id).ValueGeneratedOnAdd();
    }
}
