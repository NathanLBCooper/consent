using System.Threading;
using System.Threading.Tasks;
using Consent.Application.Workspaces;
using Consent.Domain.Core;
using Consent.Domain.Core.Errors;
using Consent.Domain.Purposes;
using static Consent.Domain.Core.Result<Consent.Domain.Purposes.Purpose>;

namespace Consent.Application.Purposes.Create;

public interface IPurposeCreateCommandHandler : ICommandHandler<PurposeCreateCommand, Result<Purpose>> { }

public class PurposeCreateCommandHandler : IPurposeCreateCommandHandler
{
    private readonly IPurposeRepository _purposeRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly PurposeCreateCommandValidator _validator = new();

    public PurposeCreateCommandHandler(IPurposeRepository purposeRepository, IWorkspaceRepository workspaceRepository)
    {
        _purposeRepository = purposeRepository;
        _workspaceRepository = workspaceRepository;
    }

    public async Task<Result<Purpose>> Handle(PurposeCreateCommand command, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return Failure(new ValidationError(validationResult.ToString()));
        }

        var workspace = await _workspaceRepository.Get(command.WorkspaceId);
        if (workspace is null)
        {
            return Failure(new NotFoundError());
        }

        if (!workspace.UserCanEdit(command.RequestedBy))
        {
            return Failure(new NotFoundError());
        }

        var created = await _purposeRepository.Create(
            new Purpose(command.WorkspaceId, Guard.NotNull(command.Name), Guard.NotNull(command.Description)));

        return Success(created);
    }
}
