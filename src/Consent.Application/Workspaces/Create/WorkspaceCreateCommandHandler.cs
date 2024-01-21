using System.Threading;
using System.Threading.Tasks;
using Consent.Application.Users;
using Consent.Domain.Core;
using Consent.Domain.Core.Errors;
using Consent.Domain.Workspaces;
using static Consent.Domain.Core.Result<Consent.Domain.Workspaces.Workspace>;

namespace Consent.Application.Workspaces.Create;

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
            return Failure(new ValidationError(validationResult.ToString()));
        }

        var user = await _userRepository.Get(command.RequestedBy);
        if (user is null)
        {
            return Failure(new UnauthorizedError());
        }

        var created = await _workspaceRepository.Create(new Workspace(Guard.NotNull(command.Name), Guard.NotNull(user.Id)));

        return Success(created);
    }
}
