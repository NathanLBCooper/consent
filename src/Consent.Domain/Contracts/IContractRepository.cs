using System.Threading.Tasks;

namespace Consent.Domain.Contracts;

public interface IContractRepository
{
    Task<ContractEntity?> Get(ContractId id);
    Task<ContractEntity> Create(Contract contract);
    Task Update(ContractEntity contract);
}
