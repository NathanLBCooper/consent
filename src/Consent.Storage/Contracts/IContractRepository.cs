using System.Threading.Tasks;
using Consent.Domain.Contracts;

namespace Consent.Storage.Contracts;

public interface IContractRepository
{
    Task<Contract?> Get(ContractId id);
    Task<Contract> Create(Contract contract);
    Task Update(Contract contract);
}
