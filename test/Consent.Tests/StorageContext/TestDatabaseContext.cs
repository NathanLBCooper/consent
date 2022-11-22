using Dapper;
using SimpleMigrations;
using SimpleMigrations.DatabaseProvider;
using System;
using System.Data.SqlClient;
using System.Reflection;

namespace Consent.Tests.StorageContext
{
    internal class TestDatabaseContext : IDisposable
    {
        private const string DataSource = "localhost,31433"; // todo for test in docker "sql-server,1433";
        private readonly string _databaseName;

        public string? ConnectionString { get; private set; }

        public TestDatabaseContext()
        {
            _databaseName = $"uow-mssql-test{Guid.NewGuid()}";
        }

        public void InitializeTestDatabase()
        {
            CreateTestDatabase(_databaseName);

            ConnectionString = GetSQLConnectionString(_databaseName);

            MigrateDatabase(ConnectionString);
        }

        private static void CreateTestDatabase(string databaseName)
        {
            var masterConnectionString = GetSQLConnectionString("master");

            using var masterConnection = new SqlConnection(masterConnectionString);
            masterConnection.Execute($"create database [{databaseName}]");
        }

        private static void MigrateDatabase(string connectionString)
        {
            using var connection = new SqlConnection(connectionString);
            var databaseProvider = new MssqlDatabaseProvider(connection);
            var migrator = new SimpleMigrator(typeof(Consent.Storage.Migrator.Program).GetTypeInfo().Assembly, databaseProvider);
            migrator.Load();
            migrator.MigrateToLatest();
        }

        public void Dispose()
        {
            var masterConnectionString = GetSQLConnectionString("master");
            using var masterConnection = new SqlConnection(masterConnectionString);

            masterConnection.Execute($"alter database [{_databaseName}] set SINGLE_USER with rollback immediate");
            masterConnection.Execute($"drop database [{_databaseName}]");
        }

        private static string GetSQLConnectionString(string database)
        {
            return new SqlConnectionStringBuilder
            {
                DataSource = DataSource,
                UserID = "sa",
                Password = "Password1!",
                InitialCatalog = database
            }.ToString();
        }
    }
}
