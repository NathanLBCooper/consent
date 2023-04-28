using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Consent.Api;

public class Program
{
    public static int Main(string[] args)
    {
        ConfigureLogging();
        var assembly = Assembly.GetExecutingAssembly().GetName();
        Log.Information("Logging Configured. Starting up {APPNAME} {VERSION}", assembly.Name, assembly?.Version?.ToString());

        try
        {
            var app = CreateHostBuilder(args).Build();
            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }

        return 0;
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(web =>
            {
                _ = web.ConfigureKestrel(options => { })
                .UseStartup<Startup>();
            }).UseSerilog();
    }

    private static void ConfigureLogging()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {SourceContext}:{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
    }
}
