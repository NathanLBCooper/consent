using System.Threading.Tasks;
using Consent.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Consent.Storage.Contacts;

public class ContractRepository : IContractRepository
{
    private readonly ContractDbContext _dbContext;

    public ContractRepository(ContractDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Contract> Create(Contract contract)
    {
        _ = await _dbContext.AddAsync(contract);
        _ = await _dbContext.SaveChangesAsync();

        return contract;
    }

    public async Task<Contract?> Get(ContractId id)
    {
        //if (await _dbContext.Contracts.FindAsync(id) is Contract contract)
        //{
        //    return contract;
        //}
        await Task.CompletedTask;

        return null;
    }

    public async Task Update(Contract contract)
    {
        _dbContext.Attach(contract).State = EntityState.Modified;
        _ = await _dbContext.SaveChangesAsync();
    }
}
