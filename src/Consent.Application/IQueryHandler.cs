using System.Threading;
using System.Threading.Tasks;

namespace Consent.Application;

public interface IQueryHandler<TQuery, TOut>
{
    Task<TOut> Handle(TQuery query, CancellationToken cancellationToken);
}
