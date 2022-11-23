using Consent.Storage;
using Consent.Storage.TypeHandlers;
using Consent.Storage.UnitOfWork;
using System;

namespace Consent.Tests.StorageContext
{
    public class DatabaseFixture : IDisposable
    {
        private readonly TestDatabaseContext _testDatabaseContext;
        private readonly SqlSettings _sqlSettings;

        public UnitOfWorkContext CreateUnitOfWorkContext() => new UnitOfWorkContext(_sqlSettings);

        public DatabaseFixture()
        {
            _testDatabaseContext = new TestDatabaseContext();
            _testDatabaseContext.InitializeTestDatabase();
            var connectionString = _testDatabaseContext.ConnectionString;

            _sqlSettings = new SqlSettings { ConnectionString = connectionString };
        }

        static DatabaseFixture()
        {
            TypeHandlers.Setup();
        }

        public void Dispose()
        {
            _testDatabaseContext.Dispose();
        }
    }
}
