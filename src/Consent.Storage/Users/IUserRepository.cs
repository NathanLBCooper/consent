using Consent.Domain.Users;

namespace Consent.Storage.Users;

public interface IUserRepository : IRepository<User, UserId>
{
}
