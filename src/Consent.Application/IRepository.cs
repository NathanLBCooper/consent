using System.Threading.Tasks;
using Consent.Domain.Core;

namespace Consent.Application;

public interface IRepository<TEntity, TIdentifier>
    where TEntity : IEntity<TIdentifier> where TIdentifier : struct, IIdentifier
{
    Task<TEntity?> Get(TIdentifier id);
    Task<TEntity> Create(TEntity entity);
    Task Update(TEntity entity);
}
