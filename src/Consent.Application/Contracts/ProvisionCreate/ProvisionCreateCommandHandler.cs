using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Consent.Application.Workspaces;
using Consent.Domain.Contracts;
using Consent.Domain.Core;
using Consent.Domain.Core.Errors;
using static Consent.Domain.Core.Result<Consent.Application.Contracts.ProvisionCreate.ProvisionCreateCommandResponse>;

namespace Consent.Application.Contracts.ProvisionCreate;

public interface IProvisionCreateCommandHandler : ICommandHandler<ProvisionCreateCommand, Result<ProvisionCreateCommandResponse>> { }

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

    public async Task<Result<ProvisionCreateCommandResponse>> Handle(ProvisionCreateCommand command, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return Failure(new ValidationError(validationResult.ToString()));
        }

        var contract = await _contractRepository.FindByContractVersion(command.ContractVersionId);
        if (contract is null)
        {
            return Failure(new NotFoundError());
        }

        var workspace = Guard.NotNull(await _workspaceRepository.Get(contract.WorkspaceId));
        if (!workspace.UserCanEdit(command.RequestedBy))
        {
            return Failure(new UnauthorizedError());
        }

        var version = contract.Versions.Single(v => v.Id == command.ContractVersionId);

        var created = new Provision(Guard.NotNull(command.Text), Guard.NotNull(command.PurposeIds));
        version.AddProvisions(created);

        await _contractRepository.Update(contract);

        return Success(new(version, created));
    }
}
