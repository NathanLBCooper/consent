using System.Threading.Tasks;

namespace Consent.Domain.Users;

public interface IUserRepository
{
    Task<UserEntity?> Get(UserId id);
    Task<UserEntity> Create(User user);
}
