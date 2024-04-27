using System.Collections.Generic;
using Consent.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Consent.Tests.Infrastructure;

/// <summary>
/// Factory for bootstrapping application in memory.
/// Uses `static IHostBuilder CreateHostBuilder(string[] args)` as entrypoint
/// </summary>
internal class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly Dictionary<string, string?> _config;

    public TestWebApplicationFactory(Dictionary<string, string?> config)
    {
        _config = config;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _ = builder.ConfigureAppConfiguration(config =>
        {
            config.Sources.Clear();

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(_config)
                .Build();
            _ = config.AddConfiguration(configuration);
        });

        _ = builder.UseEnvironment("Test");
    }
}
