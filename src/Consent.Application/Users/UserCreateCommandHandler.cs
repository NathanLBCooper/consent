using System.Threading;
using System.Threading.Tasks;
using Consent.Domain.Core;
using Consent.Domain.Core.Errors;
using Consent.Domain.Users;
using FluentValidation;

namespace Consent.Application.Users;

public record UserCreateCommand(
    string? Name
    );

public class UserCreateCommandValidator : AbstractValidator<UserCreateCommand>
{
    public UserCreateCommandValidator()
    {
        _ = RuleFor(q => q).NotEmpty();
        _ = RuleFor(q => q.Name).NotEmpty();
    }
}

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
            return Result<User>.Failure(new ValidationError(validationResult.ToString()));
        }

        var created = await _userRepository.Create(new User(Guard.NotNull(command.Name)));

        return Result<User>.Success(created);
    }
}
