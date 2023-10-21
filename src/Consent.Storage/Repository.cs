using System.Threading.Tasks;
using Consent.Domain;
using Microsoft.EntityFrameworkCore;

namespace Consent.Storage;

public abstract class Repository<TEntity, TIdentifier>
    where TEntity : class, IEntity<TIdentifier> where TIdentifier : struct, IIdentifier
{
    private readonly DbContext _dbContext;

    protected Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TEntity?> Get(TIdentifier id)
    {
        return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(e => id.Equals(e.Id));
    }

    public async Task<TEntity> Create(TEntity entity)
    {
        _ = await _dbContext.Set<TEntity>().AddAsync(entity);
        _ = await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task Update(TEntity entity)
    {
        _dbContext.Attach(entity).State = EntityState.Modified;
        _ = await _dbContext.SaveChangesAsync();
    }
}
