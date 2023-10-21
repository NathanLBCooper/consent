using System.Threading.Tasks;
using Consent.Domain;

namespace Consent.Storage;

public interface IRepository<TEntity, TIdentifier>
    where TEntity : IEntity<TIdentifier> where TIdentifier : struct, IIdentifier
{
    Task<TEntity?> Get(TIdentifier id);
    Task<TEntity> Create(TEntity entity);
    Task Update(TEntity entity);
}
