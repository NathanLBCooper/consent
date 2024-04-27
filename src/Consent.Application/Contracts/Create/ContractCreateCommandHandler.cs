using System.Threading;
using System.Threading.Tasks;
using Consent.Application.Workspaces;
using Consent.Domain.Contracts;
using Consent.Domain.Core;
using Consent.Domain.Core.Errors;
using static Consent.Domain.Core.Result<Consent.Domain.Contracts.Contract>;

namespace Consent.Application.Contracts.Create;


public interface IContractCreateCommandHandler : ICommandHandler<ContractCreateCommand, Result<Contract>>
{
}

public class ContractCreateCommandHandler : IContractCreateCommandHandler
{
    private readonly IContractRepository _contractRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly ContractCreateCommandValidator _validator = new();

    public ContractCreateCommandHandler(IContractRepository contractRepository,
        IWorkspaceRepository workspaceRepository)
    {
        _contractRepository = contractRepository;
        _workspaceRepository = workspaceRepository;
    }

    public async Task<Result<Contract>> Handle(ContractCreateCommand command, CancellationToken cancellationToken)
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
            return Failure(new UnauthorizedError());
        }

        var created =
            await _contractRepository.Create(new Contract(command.WorkspaceId, Guard.NotNull(command.Name)));

        return Success(created);
    }
}
