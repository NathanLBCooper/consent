using System;
using Serilog;
using SimpleMigrations;

namespace Consent.Storage.Migrator;

internal class MigrationLogger : SimpleMigrations.ILogger
{
    public void BeginMigration(MigrationData migration, MigrationDirection direction)
    {
        var term = direction == MigrationDirection.Up ? "migrating" : "reverting";
        Log.Information($"{migration.Version}: {migration.FullName} {term}");
    }

    public void BeginSequence(MigrationData from, MigrationData to)
    {
        Log.Information($"Migrating from {from.Version}: {from.FullName} to {to.Version}: {to.FullName}");
    }

    public void EndMigration(MigrationData migration, MigrationDirection direction)
    {
    }

    public void EndMigrationWithError(Exception exception, MigrationData migration, MigrationDirection direction)
    {
        Log.Fatal($"{migration.Version}: {migration.FullName} ERROR {exception.Message}");
    }

    public void EndSequence(MigrationData from, MigrationData to)
    {
        Log.Information("Done");
    }

    public void EndSequenceWithError(Exception exception, MigrationData from, MigrationData currentVersion)
    {
        Log.Fatal("Database is currently on version {0}: {1}", currentVersion.Version, currentVersion.FullName);
    }

    public void Info(string message)
    {
        Log.Information(message);
    }

    public void LogSql(string sql)
    {
    }
}
