using System.Collections.Generic;
using System.Threading.Tasks;
using Consent.Api;
using Consent.Api.Client.Endpoints;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Refit;
using Shouldly;
using SimpleInjector;

namespace Consent.Tests.Api;
public class ApiTest
{
    [Fact]
    public void Dependencies_should_register()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "SqlSettings:ConnectionString", "myconnectionstr" },
            })
            .Build();

        var container = new Container();
        container.Options.DefaultScopedLifestyle = ScopedLifestyle.Flowing;

        Dependencies.Register(container, config);

        container.Verify();
    }

    [Fact]
    public async Task Service_should_start()
    {
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();

        var healthEndpoint = RestService.For<IHealthEndpoint>(client);
        var health = healthEndpoint.Get;
        await health.ShouldNotThrowAsync();

        var response = await client.GetAsync("/Health");
        _ = response.EnsureSuccessStatusCode();
    }
}
