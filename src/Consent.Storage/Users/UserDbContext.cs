using Consent.Domain.Users;
using Microsoft.EntityFrameworkCore;

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

        _ = modelBuilder.Entity<User>().HasKey(e => e.Id);
        _ = modelBuilder.Entity<User>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();
    }
}
