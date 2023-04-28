using System;
using Consent.Storage;
using Consent.Storage.Contacts;
using Consent.Storage.Users;
using Consent.Storage.Workspaces;
using Microsoft.EntityFrameworkCore;

namespace Consent.Tests.Infrastructure;

public class DatabaseFixture : IDisposable
{
    private readonly TestDatabaseContext _testDatabaseContext;

    public SqlSettings SqlSettings { get; }
    public UserDbContext UserDbContext { get; }
    public WorkspaceDbContext WorkspaceDbContext { get; }
    public ContractDbContext ContractDbContext { get; }

    public DatabaseFixture()
    {
        _testDatabaseContext = new TestDatabaseContext();
        _testDatabaseContext.InitializeTestDatabase();
        var connectionString = _testDatabaseContext.ConnectionString;

        SqlSettings = new SqlSettings { ConnectionString = connectionString };

        UserDbContext = new UserDbContext(
            new DbContextOptionsBuilder<UserDbContext>().UseSqlServer(connectionString).Options
            );

        WorkspaceDbContext = new WorkspaceDbContext(
            new DbContextOptionsBuilder<WorkspaceDbContext>().UseSqlServer(connectionString).Options
            );

        ContractDbContext = new ContractDbContext(
            new DbContextOptionsBuilder<ContractDbContext>().UseSqlServer(connectionString).Options
            );
    }

    public void Dispose()
    {
        _testDatabaseContext.Dispose();
    }
}
