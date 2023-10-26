using Consent.Domain.Users;

namespace Consent.Application.Users;

public interface IUserRepository : IRepository<User, UserId>
{
}
