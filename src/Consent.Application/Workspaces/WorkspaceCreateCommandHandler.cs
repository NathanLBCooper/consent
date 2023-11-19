using System.Threading;
using System.Threading.Tasks;
using Consent.Application.Users;
using Consent.Domain.Core;
using Consent.Domain.Core.Errors;
using Consent.Domain.Users;
using Consent.Domain.Workspaces;
using FluentValidation;

namespace Consent.Application.Workspaces;

public record WorkspaceCreateCommand(
    string? Name,
    UserId RequestedBy
    );

public class WorkspaceCreateCommandValidator : AbstractValidator<WorkspaceCreateCommand>
{
    public WorkspaceCreateCommandValidator()
    {
        _ = RuleFor(q => q).NotEmpty();
        _ = RuleFor(q => q.Name).NotEmpty();
    }
}

public interface IWorkspaceCreateCommandHandler : ICommandHandler<WorkspaceCreateCommand, Result<Workspace>> { }

public class WorkspaceCreateCommandHandler : IWorkspaceCreateCommandHandler
{
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IUserRepository _userRepository;
    private readonly WorkspaceCreateCommandValidator _validator = new();

    public WorkspaceCreateCommandHandler(IWorkspaceRepository workspaceRepository, IUserRepository userRepository)
    {
        _workspaceRepository = workspaceRepository;
        _userRepository = userRepository;
    }

    public async Task<Result<Workspace>> Handle(WorkspaceCreateCommand command, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return Result<Workspace>.Failure(new ValidationError(validationResult.ToString()));
        }

        var user = await _userRepository.Get(command.RequestedBy);
        if (user is null)
        {
            return Result<Workspace>.Failure(new UnauthorizedError());
        }

        var created = await _workspaceRepository.Create(new Workspace(Guard.NotNull(command.Name), Guard.NotNull(user.Id)));

        return Result<Workspace>.Success(created);
    }
}
