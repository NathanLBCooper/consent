using System;
using System.Linq;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleInjector;

namespace Consent.Api;

internal class Startup
{
    private readonly IConfiguration _configuration;
    private readonly Container _container = new();

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        _ = services
            .AddControllers()
            .AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
            );

        _ = services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen();

        _ = services.AddHealthChecks();

        _ = services.AddSimpleInjector(_container, options =>
        {
            _ = options.AddAspNetCore().AddControllerActivation();
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            _ = app.UseDeveloperExceptionPage();
            _ = app.UseSwagger().UseSwaggerUI();
        }

        _ = app.UseCors(builder =>
        {
            var allowedHosts = new[] { "localhost" };
            _ = builder.SetIsOriginAllowed(origin => allowedHosts.Contains(new Uri(origin).Host));
        });

        _ = app.UseRouting();
        _ = app.UseAuthentication();

        _ = app.UseHealthChecks("/health");
        _ = app.UseEndpoints(endpoints =>
        {
            _ = endpoints.MapControllers();
        });

        Dependencies.Register(_container, _configuration);
        _ = app.UseSimpleInjector(_container);
        _container.Verify();
    }
}
