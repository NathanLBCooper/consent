using Consent.Domain.UnitOfWork;
using Consent.Storage;
using Consent.Storage.UnitOfWork;
using System;
using System.Data;

namespace Consent.Tests.StorageContext
{
    public class DatabaseFixture : IDisposable
    {
        private readonly TestDatabaseContext _testDatabaseContext;

        public IGetConnection GetConnection { get; }
        public ICreateUnitOfWork CreateUnitOfWork { get; }

        public DatabaseFixture()
        {
            _testDatabaseContext = new TestDatabaseContext();
            _testDatabaseContext.InitializeTestDatabase();
            var connectionString = _testDatabaseContext.ConnectionString;

            var sqlSettings = new SqlSettings { ConnectionString = connectionString };

            var unitOfWorkContext = new UnitOfWorkContext(sqlSettings);
            CreateUnitOfWork = unitOfWorkContext;
            GetConnection = unitOfWorkContext;
        }

        public (IDbConnection connection, IDbTransaction transaction) GetCurrentConnection() => GetConnection.GetConnection();

        public void Dispose()
        {
            _testDatabaseContext.Dispose();
        }
    }
}
