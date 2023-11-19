using System.Threading;
using System.Threading.Tasks;
using Consent.Application.Workspaces;
using Consent.Domain.Contracts;
using Consent.Domain.Core;

namespace Consent.Application.Contracts.Get;

public interface IContractGetQueryHandler : IQueryHandler<ContractGetQuery, Maybe<Contract>> { }

public class ContractGetQueryHandler : IContractGetQueryHandler
{
    private readonly IContractRepository _contractRepository;
    private readonly IWorkspaceRepository _workspaceRepository;

    public ContractGetQueryHandler(IContractRepository contractRepository, IWorkspaceRepository workspaceRepository)
    {
        _contractRepository = contractRepository;
        _workspaceRepository = workspaceRepository;
    }

    public async Task<Maybe<Contract>> Handle(ContractGetQuery query, CancellationToken cancellationToken)
    {
        var contract = await _contractRepository.Get(query.ContractId);
        if (contract is null)
        {
            return Maybe<Contract>.None;
        }

        var workspace = Guard.NotNull(await _workspaceRepository.Get(contract.WorkspaceId));
        if (!workspace.UserCanView(query.RequestedBy))
        {
            return Maybe<Contract>.None;
        }

        return Maybe<Contract>.Some(contract);
    }
}
