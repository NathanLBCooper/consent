using System;
using Consent.Storage.Contracts;
using Consent.Storage.Purposes;
using Consent.Storage.Users;
using Consent.Storage.Workspaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Consent.Storage.Migrations;

/*
 *  This class is just here to allow `dotnet ef migrations add [MigrationName]` to run on this class libary
 */
internal class MigrationToolDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>, IDesignTimeDbContextFactory<WorkspaceDbContext>,
    IDesignTimeDbContextFactory<PurposeDbContext>, IDesignTimeDbContextFactory<ContractDbContext>
{
    UserDbContext IDesignTimeDbContextFactory<UserDbContext>.CreateDbContext(string[] args)
    {
        return new UserDbContext(BuildOptions<UserDbContext>().Options);
    }

    WorkspaceDbContext IDesignTimeDbContextFactory<WorkspaceDbContext>.CreateDbContext(string[] args)
    {
        return new WorkspaceDbContext(BuildOptions<WorkspaceDbContext>().Options);
    }

    PurposeDbContext IDesignTimeDbContextFactory<PurposeDbContext>.CreateDbContext(string[] args)
    {
        return new PurposeDbContext(BuildOptions<PurposeDbContext>().Options);
    }

    ContractDbContext IDesignTimeDbContextFactory<ContractDbContext>.CreateDbContext(string[] args)
    {
        return new ContractDbContext(BuildOptions<ContractDbContext>().Options);
    }

    private static DbContextOptionsBuilder<TDbContext> BuildOptions<TDbContext>() where TDbContext : DbContext
    {
        return new DbContextOptionsBuilder<TDbContext>()
            .UseSqlServer("this is a fake connection string")
            .ReplaceService<ISqlGenerationHelper, NoGoSqlGenerationHelper>();
    }

#pragma warning disable EF1001 // Internal EF Core API usage.
    private class NoGoSqlGenerationHelper : SqlServerSqlGenerationHelper
    {
        public NoGoSqlGenerationHelper(RelationalSqlGenerationHelperDependencies dependencies)
             : base(dependencies)
        {
        }

        // Avoids generating GO in scripts
        public override string BatchTerminator => Environment.NewLine;
    }
#pragma warning restore EF1001 // Internal EF Core API usage.

}
