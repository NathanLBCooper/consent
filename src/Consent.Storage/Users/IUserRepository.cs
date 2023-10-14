using System.Threading.Tasks;
using Consent.Domain.Users;

namespace Consent.Storage.Users;

public interface IUserRepository
{
    Task<User?> Get(UserId id);
    Task<User> Create(User user);
}
