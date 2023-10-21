using Consent.Domain.Contracts;

namespace Consent.Storage.Contracts;

public class ContractRepository : Repository<Contract, ContractId>, IContractRepository
{
    public ContractRepository(ContractDbContext dbContext) : base(dbContext)
    {
    }
}
