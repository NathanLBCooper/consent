using Consent.Domain.UnitOfWork;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Consent.Storage.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IGetConnection, IDisposable
    {
        private readonly SqlConnection _connection;
        private readonly SqlTransaction _transaction;

        public bool IsDisposed { get; private set; } = false;

        public UnitOfWork(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public async Task RollBackAsync()
        {
            await _transaction.RollbackAsync();
        }

        public async Task CommitAsync()
        {
            await _transaction.CommitAsync();
        }

        public (IDbConnection connection, IDbTransaction transaction) GetConnection()
        {
            return (_connection, _transaction);
        }

        public void Dispose()
        {
            _connection?.Dispose();
            _transaction?.Dispose();

            IsDisposed = true;
        }
    }
}
