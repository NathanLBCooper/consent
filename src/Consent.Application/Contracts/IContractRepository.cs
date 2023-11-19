using System.Threading.Tasks;
using Consent.Domain.Contracts;

namespace Consent.Application.Contracts;

public interface IContractRepository : IRepository<Contract, ContractId>
{
    Task<Contract?> FindByContractVersion(ContractVersionId contractVersionId);
}
