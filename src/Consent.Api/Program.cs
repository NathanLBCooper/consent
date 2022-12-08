using System;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SimpleInjector;

namespace Consent.Api;

public class Program
{
    public static int Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        ConfigureLogging(configuration);
        Storage.TypeHandlers.TypeHandlers.Setup();

        try
        {
            var builder = WebApplication.CreateBuilder(args);
            _ = builder.Host.UseSerilog();

            _ = builder.Services.AddControllers().AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
            );

            _ = builder.Services.AddEndpointsApiExplorer();
            _ = builder.Services.AddSwaggerGen();

            var container = new Container();
            _ = builder.Services.AddSimpleInjector(container, options =>
            {
                _ = options.AddAspNetCore().AddControllerActivation();
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                _ = app.UseSwagger();
                _ = app.UseSwaggerUI();
            }

            _ = app.Services.UseSimpleInjector(container);
            _ = app.UseAuthorization();
            _ = app.MapControllers();

            Dependencies.Register(container, configuration);
            container.Verify();

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
}