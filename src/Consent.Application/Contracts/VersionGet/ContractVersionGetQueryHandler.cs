using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Consent.Application.Workspaces;
using Consent.Domain.Core;
using static Consent.Domain.Core.Maybe<Consent.Application.Contracts.VersionGet.ContractVersionGetQueryResponse>;

namespace Consent.Application.Contracts.VersionGet;

public interface IContractVersionGetQueryHandler : IQueryHandler<ContractVersionGetQuery, Maybe<ContractVersionGetQueryResponse>> { }

public class ContractVersionGetQueryHandler : IContractVersionGetQueryHandler
{
    private readonly IContractRepository _contractRepository;
    private readonly IWorkspaceRepository _workspaceRepository;

    public ContractVersionGetQueryHandler(IContractRepository contractRepository, IWorkspaceRepository workspaceRepository)
    {
        _contractRepository = contractRepository;
        _workspaceRepository = workspaceRepository;
    }

    public async Task<Maybe<ContractVersionGetQueryResponse>> Handle(ContractVersionGetQuery query, CancellationToken cancellationToken)
    {
        var contract = await _contractRepository.FindByContractVersion(query.ContractVersionId);
        if (contract is null)
        {
            return None;
        }

        var version = contract.Versions.SingleOrDefault(v => v.Id == query.ContractVersionId);
        if (version is null)
        {
            return None;
        }

        var workspace = Guard.NotNull(await _workspaceRepository.Get(contract.WorkspaceId));
        if (!workspace.UserCanView(query.RequestedBy))
        {
            return None;
        }

        return Some(new(contract, version));
    }
}
