using System.Threading.Tasks;

namespace Consent.Domain.Users;

public interface IUserRepository
{
    Task<User?> Get(UserId id);
    Task<User> Create(User user);
}
