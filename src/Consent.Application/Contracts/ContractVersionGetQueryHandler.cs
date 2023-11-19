using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Consent.Application.Workspaces;
using Consent.Domain.Contracts;
using Consent.Domain.Core;
using Consent.Domain.Users;

namespace Consent.Application.Contracts;

public record ContractVersionGetQuery(
    ContractVersionId ContractVersionId,
    UserId RequestedBy
    );

public record ContractVersionGetQueryResult(
    Contract Contract,
    ContractVersion Version
    );

public interface IContractVersionGetQueryHandler : IQueryHandler<ContractVersionGetQuery, Maybe<ContractVersionGetQueryResult>> { }

public class ContractVersionGetQueryHandler : IContractVersionGetQueryHandler
{
    private readonly IContractRepository _contractRepository;
    private readonly IWorkspaceRepository _workspaceRepository;

    public ContractVersionGetQueryHandler(IContractRepository contractRepository, IWorkspaceRepository workspaceRepository)
    {
        _contractRepository = contractRepository;
        _workspaceRepository = workspaceRepository;
    }

    public async Task<Maybe<ContractVersionGetQueryResult>> Handle(ContractVersionGetQuery query, CancellationToken cancellationToken)
    {
        var contract = await _contractRepository.FindByContractVersion(query.ContractVersionId);
        if (contract is null)
        {
            return Maybe<ContractVersionGetQueryResult>.None;
        }

        var version = contract.Versions.SingleOrDefault(v => v.Id == query.ContractVersionId);
        if (version is null)
        {
            return Maybe<ContractVersionGetQueryResult>.None;
        }

        var workspace = Guard.NotNull(await _workspaceRepository.Get(contract.WorkspaceId));
        if (!workspace.UserCanView(query.RequestedBy))
        {
            return Maybe<ContractVersionGetQueryResult>.None;
        }

        return Maybe<ContractVersionGetQueryResult>.Some(new(contract, version));
    }
}
