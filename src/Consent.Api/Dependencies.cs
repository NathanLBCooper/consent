using System;
using System.Linq;
using System.Reflection;
using Consent.Storage;
using Consent.Storage.Contacts;
using Consent.Storage.Users;
using Consent.Storage.Workspaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SimpleInjector;

namespace Consent.Api;

public static class Dependencies
{
    public static void Register(Container container, IConfiguration configuration)
    {
        var sqlSettings = configuration.GetSection(nameof(SqlSettings)).Get<SqlSettings>();
        if (sqlSettings == null)
        {
            throw new ArgumentNullException(nameof(SqlSettings));
        }

        container.RegisterInstance<SqlSettings>(sqlSettings);

        var userDbContextOptions = Options<UserDbContext>(sqlSettings);
        container.Register<UserDbContext>(() => new UserDbContext(userDbContextOptions), Lifestyle.Scoped);

        var workspaceDbContextOptions = Options<WorkspaceDbContext>(sqlSettings);
        container.Register<WorkspaceDbContext>(() => new WorkspaceDbContext(workspaceDbContextOptions), Lifestyle.Scoped);

        var contractDbContextOptions = Options<ContractDbContext>(sqlSettings);
        container.Register<ContractDbContext>(() => new ContractDbContext(contractDbContextOptions), Lifestyle.Scoped);

        RegisterByConvention(typeof(UserRepository).Assembly, container, t => t.Name.EndsWith("Repository"));
    }

    private static DbContextOptions<TDbContext> Options<TDbContext>(SqlSettings sqlSettings) where TDbContext : DbContext
    {
        return new DbContextOptionsBuilder<TDbContext>()
            .UseSqlServer(sqlSettings.ConnectionString)
            .Options;
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
