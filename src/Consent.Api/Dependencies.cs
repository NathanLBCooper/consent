using System;
using System.Linq;
using System.Reflection;
using Consent.Domain.UnitOfWork;
using Consent.Storage;
using Consent.Storage.UnitOfWork;
using Microsoft.Extensions.Configuration;
using SimpleInjector;

namespace Consent.Api;

internal static class Dependencies
{
    internal static void Register(Container container, IConfiguration configuration)
    {
        var sqlSettings = configuration.GetSection(nameof(SqlSettings)).Get<SqlSettings>();
        if (sqlSettings == null)
        {
            throw new ArgumentNullException(nameof(SqlSettings));
        }

        container.RegisterInstance<SqlSettings>(sqlSettings);
        var uowRegistration = Lifestyle.Scoped.CreateRegistration<UnitOfWorkContext>(
            () => new UnitOfWorkContext(container.GetInstance<SqlSettings>()), container);
        container.AddRegistration(typeof(ICreateUnitOfWork), uowRegistration);
        container.AddRegistration(typeof(IGetConnection), uowRegistration);
        RegisterByConvention(typeof(Storage.Repositories.UserRepository).Assembly, container, t => t.Name.EndsWith("Repository"));
    }

    private static void RegisterByConvention(Assembly assembly, Container container, Func<Type, bool> condition)
    {
        var registrations =
            from type in assembly.GetExportedTypes()
            where condition(type)
            from service in type.GetInterfaces()
            select new { service, implementation = type };

        foreach (var reg in registrations)
        {
            container.Register(reg.service, reg.implementation, Lifestyle.Scoped);
        }
    }
}
