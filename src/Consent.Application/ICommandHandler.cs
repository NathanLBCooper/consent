using System.Threading;
using System.Threading.Tasks;

namespace Consent.Application;

public interface ICommandHandler<TCommand, TOut>
{
    Task<TOut> Handle(TCommand command, CancellationToken cancellationToken);
}
