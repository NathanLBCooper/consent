﻿using System;
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
        var validationResult = _validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return Result<ContractVersionCreateCommandResult>.Failure(new ValidationError(validationResult.ToString()));
        }

        var contract = await _contractRepository.Get(command.ContractId);
        if (contract is null)
        {
            return Result<ContractVersionCreateCommandResult>.Failure(new NotFoundError());
        }

        var workspace = Guard.NotNull(await _workspaceRepository.Get(contract.WorkspaceId));
        if (!workspace.UserCanEdit(command.RequestedBy))
        {
            return Result<ContractVersionCreateCommandResult>.Failure(new UnauthorizedError());
        }

        var created = new ContractVersion(
            Guard.NotNull(command.Name), Guard.NotNull(command.Text), Array.Empty<Provision>()
            );
        contract.AddContractVersions(created);

        await _contractRepository.Update(contract);

        return Result<ContractVersionCreateCommandResult>.Success(new(contract, created));
    }
}