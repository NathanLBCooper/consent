using System.Reflection;
using System.Linq;
using SimpleInjector;
using Consent.Storage.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Consent.Storage;
using System;
using Consent.Domain.UnitOfWork;

namespace Consent.Api
{
    internal static class Dependencies
    {
        internal static void Register(Container container, IConfiguration configuration)
        {
            var sqlSettings = configuration.GetSection(nameof(SqlSettings)).Get<SqlSettings>();
            if (sqlSettings == null) throw new ArgumentNullException(nameof(SqlSettings));
            container.RegisterInstance<SqlSettings>(sqlSettings);
            var uowRegistration = Lifestyle.Scoped.CreateRegistration<UnitOfWorkContext>(
                () => new UnitOfWorkContext(container.GetInstance<SqlSettings>()), container);
            container.AddRegistration(typeof(ICreateUnitOfWork), uowRegistration);
            container.AddRegistration(typeof(IGetConnection), uowRegistration);
            RegisterByConvention(typeof(Consent.Storage.Users.UserRepository).Assembly, container, t => t.Name.EndsWith("Repository"));

            RegisterByConvention(typeof(Consent.Domain.Users.UserEndpoint).Assembly, container, t => t.Name.EndsWith("Endpoint"));
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
}
