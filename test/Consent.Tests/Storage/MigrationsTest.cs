using System;
using System.Data.SqlClient;
using System.Reflection;
using Consent.Tests.Infrastructure;
using SimpleMigrations;
using SimpleMigrations.DatabaseProvider;

namespace Consent.Tests.Storage;

public class MigrationsTest : IDisposable
{
    private readonly DatabaseFixture _fixture;

    public MigrationsTest()
    {
        _fixture = new DatabaseFixture();
    }

    [Fact]
    public void Removing_all_migrations_runs_without_errors()
    {
        using var connection = new SqlConnection(_fixture.SqlSettings.ConnectionString);
        var databaseProvider = new MssqlDatabaseProvider(connection);
        var migrator = new SimpleMigrator(typeof(Consent.Storage.Migrator.Program).GetTypeInfo().Assembly, databaseProvider);
        migrator.Load();

        migrator.MigrateTo(0);
    }

    public void Dispose()
    {
        _fixture.Dispose();
    }
}
