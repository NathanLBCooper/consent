using Consent.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Consent.Storage.Users;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
        this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public DbSet<User> Users => Set<User>();
}
