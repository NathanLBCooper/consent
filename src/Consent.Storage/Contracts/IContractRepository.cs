using Consent.Domain.Contracts;

namespace Consent.Storage.Contracts;

public interface IContractRepository : IRepository<Contract, ContractId>
{
}
