using System;
using System.Threading;
using System.Threading.Tasks;
using Consent.Application.Workspaces;
using Consent.Domain.Contracts;
using Consent.Domain.Core;
using Consent.Domain.Core.Errors;

namespace Consent.Application.Contracts.VersionCreate;

public interface IContractVersionCreateCommandHandler : ICommandHandler<ContractVersionCreateCommand, Result<ContractVersionCreateCommandResult>> { }

public class ContractVersionCreateCommandHandler : IContractVersionCreateCommandHandler
{
    private readonly IContractRepository _contractRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly ContractVersionCreateCommandValidator _validator = new();

    public ContractVersionCreateCommandHandler(IContractRepository contractRepository, IWorkspaceRepository workspaceRepository)
    {
        _contractRepository = contractRepository;
        _workspaceRepository = workspaceRepository;
    }

    public async Task<Result<ContractVersionCreateCommandResult>> Handle(ContractVersionCreateCommand command, CancellationToken cancellationToken)
    {
        return await _validator.Validate(command).ToResult()
            .Then(async () =>
            {
                var contract = await _contractRepository.Get(command.ContractId);
                if (contract is null)
                {
                    return Result<Contract>.Failure(new NotFoundError());
                }

                return Result<Contract>.Success(contract);
            })
            .Then(async (Contract contract) =>
            {
                var workspace = Guard.NotNull(await _workspaceRepository.Get(contract.WorkspaceId));

                if (!workspace.UserCanEdit(command.RequestedBy))
                {
                    return Result<Contract>.Failure(new UnauthorizedError());
                }

                return Result<Contract>.Success(contract);
            })
            .Then((Contract contract) =>
            {
                return ContractVersion.New(Guard.NotNull(command.Name), Guard.NotNull(command.Text), Array.Empty<Provision>())
                    .Then(version => Result<(Contract contract, ContractVersion version)>.Success((contract, version)));
            }).Then(async (r) =>
            {
                var (contract, created) = r;

                contract.AddContractVersions(created);

                await _contractRepository.Update(contract);
                return Result<ContractVersionCreateCommandResult>.Success(new ContractVersionCreateCommandResult(contract, created));
            });
    }
}
