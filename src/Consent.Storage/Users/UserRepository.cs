using Consent.Application.Users;
using Consent.Domain.Users;

namespace Consent.Storage.Users;

public class UserRepository : Repository<User, UserId>, IUserRepository
{
    public UserRepository(UserDbContext dbContext) : base(dbContext)
    {
    }
}
