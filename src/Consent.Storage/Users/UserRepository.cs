using System.Threading.Tasks;
using Consent.Domain.Users;

namespace Consent.Storage.Users;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _dbContext;

    public UserRepository(UserDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> Get(UserId id)
    {
        if (await _dbContext.Users.FindAsync(id) is User user)
        {
            return user;
        }

        return null;
    }

    public async Task<User> Create(User user)
    {
        _ = await _dbContext.AddAsync(user);
        _ = await _dbContext.SaveChangesAsync();

        return user;
    }
}
