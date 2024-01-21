using System;
using System.Linq;
using System.Reflection;
using Consent.Application.Users.Create;
using Consent.Application.Users.Get;
using Consent.Storage;
using Consent.Storage.Contracts;
using Consent.Storage.Participants;
using Consent.Storage.Purposes;
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
        if (sqlSettings is null)
        {
            throw new ArgumentNullException(nameof(SqlSettings));
        }

        container.RegisterInstance<SqlSettings>(sqlSettings);

        var userDbContextOptions = Options<UserDbContext>(sqlSettings);
        container.Register<UserDbContext>(() => new UserDbContext(userDbContextOptions), Lifestyle.Scoped);

        var workspaceDbContextOptions = Options<WorkspaceDbContext>(sqlSettings);
        container.Register<WorkspaceDbContext>(() => new WorkspaceDbContext(workspaceDbContextOptions), Lifestyle.Scoped);

        var purposeDbContextOptions = Options<PurposeDbContext>(sqlSettings);
        container.Register<PurposeDbContext>(() => new PurposeDbContext(purposeDbContextOptions), Lifestyle.Scoped);

        var contractDbContextOptions = Options<ContractDbContext>(sqlSettings);
        container.Register<ContractDbContext>(() => new ContractDbContext(contractDbContextOptions), Lifestyle.Scoped);

        var participantDbContextOptions = Options<ParticipantDbContext>(sqlSettings);
        container.Register<ParticipantDbContext>(() => new ParticipantDbContext(participantDbContextOptions), Lifestyle.Scoped);

        RegisterByConvention(typeof(UserRepository).Assembly, container, t => t.Name.EndsWith("Repository"), true);
        RegisterByConvention(typeof(UserGetQueryHandler).Assembly, container, t => t.Name.EndsWith("QueryHandler"), true);
        RegisterByConvention(typeof(UserCreateCommandHandler).Assembly, container, t => t.Name.EndsWith("CommandHandler"), true);
    }

    private static DbContextOptions<TDbContext> Options<TDbContext>(SqlSettings sqlSettings) where TDbContext : DbContext
    {
        return new DbContextOptionsBuilder<TDbContext>()
            .UseSqlServer(sqlSettings.ConnectionString)
            .Options;
    }

    private static void RegisterByConvention(Assembly assembly, Container container,
        Func<Type, bool> condition, bool onlyMatchingNames)
    {
        var registrations = assembly
                            .GetExportedTypes()
                            .Where(t => !(t.IsInterface || t.IsAbstract || t.IsGenericTypeDefinition))
                            .Where(condition)
                            .SelectMany(t => t.GetInterfaces(), (implementation, service) => new { service, implementation });

        if (onlyMatchingNames)
        {
            // "IFoo" and "Foo" are examples of matching names
            registrations = registrations.Where(p => p.implementation.Name == p.service.Name[1..]);
        }

        foreach (var reg in registrations)
        {
            container.Register(reg.service, reg.implementation, Lifestyle.Scoped);
        }
    }
}
