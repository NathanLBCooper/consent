using System.Threading.Tasks;
using Consent.Domain.Users;
using Microsoft.EntityFrameworkCore;

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
        return await _dbContext.Users.FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<User> Create(User user)
    {
        _ = await _dbContext.AddAsync(user);
        _ = await _dbContext.SaveChangesAsync();

        return user;
    }
}
