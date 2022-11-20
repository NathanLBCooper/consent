using System;
using System.Threading.Tasks;

namespace Consent.Domain.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task RollBackAsync();
        Task CommitAsync();
        bool IsDisposed { get; }
    }
}
