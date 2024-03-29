using System.Threading;
using System.Threading.Tasks;
using Consent.Domain.Core;
using Consent.Domain.Users;
using static Consent.Domain.Core.Maybe<Consent.Domain.Users.User>;

namespace Consent.Application.Users.Get;

public interface IUserGetQueryHandler : IQueryHandler<UserGetQuery, Maybe<User>> { }

public class UserGetQueryHandler : IUserGetQueryHandler
{
    private readonly IUserRepository _userRepository;

    public UserGetQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Maybe<User>> Handle(UserGetQuery command, CancellationToken cancellationToken)
    {
        if (command.UserId != command.RequestedBy)
        {
            return None;
        }

        var user = await _userRepository.Get(command.UserId);
        if (user is null)
        {
            return None;
        }

        return Some(user);
    }
}
