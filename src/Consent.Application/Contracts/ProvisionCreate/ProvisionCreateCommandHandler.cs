using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Consent.Application.Workspaces;
using Consent.Domain.Contracts;
using Consent.Domain.Core;
using Consent.Domain.Core.Errors;

namespace Consent.Application.Contracts.ProvisionCreate;

public interface IProvisionCreateCommandHandler : ICommandHandler<ProvisionCreateCommand, Result<ProvisionCreateCommandResult>> { }

public class ProvisionCreateCommandHandler : IProvisionCreateCommandHandler
{
    private readonly IContractRepository _contractRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly ProvisionCreateCommandValidator _validator = new();

    public ProvisionCreateCommandHandler(IContractRepository contractRepository, IWorkspaceRepository workspaceRepository)
    {
        _contractRepository = contractRepository;
        _workspaceRepository = workspaceRepository;
    }

    public async Task<Result<ProvisionCreateCommandResult>> Handle(ProvisionCreateCommand command, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return Result<ProvisionCreateCommandResult>.Failure(new ValidationError(validationResult.ToString()));
        }

        var contract = await _contractRepository.FindByContractVersion(command.ContractVersionId);
        if (contract is null)
        {
            return Result<ProvisionCreateCommandResult>.Failure(new NotFoundError());
        }

        var workspace = Guard.NotNull(await _workspaceRepository.Get(contract.WorkspaceId));
        if (!workspace.UserCanEdit(command.RequestedBy))
        {
            return Result<ProvisionCreateCommandResult>.Failure(new UnauthorizedError());
        }

        var version = contract.Versions.Single(v => v.Id == command.ContractVersionId);

        var created = new Provision(Guard.NotNull(command.Text), Guard.NotNull(command.PermissionIds));
        version.AddProvisions(created);

        await _contractRepository.Update(contract);

        return Result<ProvisionCreateCommandResult>.Success(new(version, created));
    }
}
