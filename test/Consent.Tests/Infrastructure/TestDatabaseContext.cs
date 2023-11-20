using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Dapper;
using SimpleMigrations;
using SimpleMigrations.DatabaseProvider;

namespace Consent.Tests.Infrastructure;

internal class TestDatabaseContext : IDisposable
{
    private const string DataSource = "localhost,31433"; // todo for test in docker "sql-server,1433";
    private readonly string _databaseName;

    public string? ConnectionString { get; private set; }

    public TestDatabaseContext()
    {
        _databaseName = $"consent-db-test{Guid.NewGuid()}";
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
        _ = masterConnection.Execute($"create database [{databaseName}]");
    }

    private static void MigrateDatabase(string connectionString)
    {
        using var connection = new SqlConnection(connectionString);
        var databaseProvider = new MssqlDatabaseProvider(connection);
        var migrator = new SimpleMigrator(typeof(Consent.Storage.Migrator.Program).GetTypeInfo().Assembly, databaseProvider);
        migrator.Load();
        migrator.MigrateToLatest();

        DisjoinIdRanges(connection);
    }

    public void Dispose()
    {
        var masterConnectionString = GetSQLConnectionString("master");
        using var masterConnection = new SqlConnection(masterConnectionString);

        _ = masterConnection.Execute($"alter database [{_databaseName}] set SINGLE_USER with rollback immediate");
        _ = masterConnection.Execute($"drop database [{_databaseName}]");
    }

    private static string GetSQLConnectionString(string database)
    {
        return new SqlConnectionStringBuilder
        {
            DataSource = DataSource,
            UserID = "sa",
            Password = "Password1!",
            InitialCatalog = database,
            TrustServerCertificate = true,
        }.ToString();
    }

    /**
     * Avoid collisions between the ids of different entities
     */
    private static void DisjoinIdRanges(IDbConnection connection)
    {
        var tables = connection.Query<(string schema, string name, string type)>($"select table_schema, table_name, table_type FROM information_schema.tables")
            .Where(t => t.type == "BASE TABLE") // ie, not a VIEW
            .Where(t => t.name != "VersionInfo")
            .Select(t => $"[{t.schema}].[{t.name}]");

        var startId = 10000;
        const int step = 10000;
        foreach (var table in tables)
        {
            // todo might break when there are tables with int identity
            _ = connection.Execute($"dbcc checkident ('{table}', reseed, {startId})");
            startId = startId + step;
        }
    }
}
