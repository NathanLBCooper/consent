using System.Linq;
using System.Threading.Tasks;
using Consent.Application.Contracts;
using Consent.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Consent.Storage.Contracts;

public class ContractRepository : Repository<Contract, ContractId>, IContractRepository
{
    private readonly ContractDbContext _dbContext;

    public ContractRepository(ContractDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Contract?> FindByContractVersion(ContractVersionId contractVersionId)
    {
        return await _dbContext.Contracts.FirstOrDefaultAsync(
            contract => contract.Versions.Any(version => version.Id == contractVersionId)
            );
    }
}
