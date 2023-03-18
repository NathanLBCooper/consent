using System.Threading.Tasks;

namespace Consent.Domain.Contracts;

public interface IContractRepository
{
    Task<Contract?> Get(ContractId id);
    Task<Contract> Create(Contract contract);
    Task Update(Contract contract);
}
