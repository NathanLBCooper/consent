using System.Threading.Tasks;
using Consent.Domain.Contracts;
using Consent.Storage.UnitOfWork;

namespace Consent.Storage.Repositories;
internal class ContractRepository : IContractRepository
{
    private readonly IGetConnection _getConnection;

    public ContractRepository(IGetConnection getConnection)
    {
        _getConnection = getConnection;
    }

    public Task<ContractEntity> Create(Contract contract)
    {
        throw new System.NotImplementedException();
    }

    public Task<ContractEntity?> Get(ContractId id)
    {
        throw new System.NotImplementedException();
    }

    public Task Update(ContractEntity contract)
    {
        throw new System.NotImplementedException();
    }
}
