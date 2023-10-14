using System;
using System.Data.SqlClient;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Serilog;
using SimpleMigrations;
using SimpleMigrations.DatabaseProvider;

namespace Consent.Storage.Migrator;

public class Program
{
    internal static int Main()
    {
        var configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json")
           .AddEnvironmentVariables()
           .Build();

        ConfigureLogging(configuration);

        try
        {
            var sqlSettings = GetConfig<SqlSettings>(configuration);
            MigrateDatabase(sqlSettings);

            Log.Information("Exited without errors");
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }

        return 0;
    }

    private static void MigrateDatabase(SqlSettings settings)
    {
        var migrationsAssembly = typeof(Migrations.Migration001_Initial).Assembly;
        using var connection = new SqlConnection(settings.ConnectionString);
        var databaseProvider = new MssqlDatabaseProvider(connection);
        var migrator = new SimpleMigrator(migrationsAssembly, databaseProvider) { Logger = new MigrationLogger() };

        migrator.Load();
        migrator.MigrateToLatest();
    }

    private static void ConfigureLogging(IConfigurationRoot configuration)
    {
        Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {SourceContext}:{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

        var assembly = Assembly.GetExecutingAssembly().GetName();
        Log.Information("Logging Configured. Starting up {APPNAME} {VERSION}", assembly.Name, assembly?.Version?.ToString());
    }

    private static T GetConfig<T>(IConfiguration configuration) where T : class
    {
        var section = configuration.GetSection(typeof(T).Name);
        var config = section.Get<T>();
        return config ?? throw new ArgumentNullException(typeof(T).Name);
    }
}
