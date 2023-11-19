using System.Threading;
using System.Threading.Tasks;
using Consent.Application.Users;
using Consent.Domain.Contracts;
using Consent.Domain.Core;

namespace Consent.Application.Contracts;

public interface IContractGetQueryHandler : IQueryHandler<ContractGetQuery, Maybe<Contract>> { }

public class ContractGetQueryHandler : IContractGetQueryHandler
{
    private readonly IContractRepository _contractRepository;
    private readonly IUserRepository _userRepository;

    public ContractGetQueryHandler(IContractRepository contractRepository, IUserRepository userRepository)
    {
        _contractRepository = contractRepository;
        _userRepository = userRepository;
    }

    public async Task<Maybe<Contract>> Handle(ContractGetQuery query, CancellationToken cancellationToken)
    {
        var user = await _userRepository.Get(query.RequestedBy);
        if (user == null)
        {
            return Maybe<Contract>.None;
        }

        var contract = await _contractRepository.Get(query.ContractId);
        if (contract == null)
        {
            return Maybe<Contract>.None;
        }

        // todo go through contract
        if (!user.CanViewWorkspace(contract.WorkspaceId))
        {
            return Maybe<Contract>.None;
        }

        return Maybe<Contract>.Some(contract);
    }
}
