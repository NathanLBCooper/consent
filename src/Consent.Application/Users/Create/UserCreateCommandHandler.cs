using System.Threading;
using System.Threading.Tasks;
using Consent.Domain.Core;
using Consent.Domain.Core.Errors;
using Consent.Domain.Users;
using static Consent.Domain.Core.Result<Consent.Domain.Users.User>;

namespace Consent.Application.Users.Create;

public interface IUserCreateCommandHandler : ICommandHandler<UserCreateCommand, Result<User>> { }

public class UserCreateCommandHandler : IUserCreateCommandHandler
{
    private readonly IUserRepository _userRepository;
    private readonly UserCreateCommandValidator _validator = new();

    public UserCreateCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<User>> Handle(UserCreateCommand command, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return Failure(new ValidationError(validationResult.ToString()));
        }

        var created = await _userRepository.Create(new User(Guard.NotNull(command.Name)));

        return Success(created);
    }
}
