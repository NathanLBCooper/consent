How to create these Migrations:

1.  Make your changes to the code (to the DBContext)

2.  If adding a new DBContext, add it to `MigrationToolDbContextFactory`

3.  In `/src/Consent.Storage`, call `dotnet ef` to generate the EF Migrations for each of the Contexts

         dotnet ef migrations add NameOfTheNewMigration --context UserDbContext --output-dir Migrations/Users
         dotnet ef migrations add NameOfTheNewMigration --context WorkspaceDbContext --output-dir Migrations/Workspaces
         dotnet ef migrations add NameOfTheNewMigration --context PurposeDbContext --output-dir Migrations/Purposes
         dotnet ef migrations add NameOfTheNewMigration --context ContractDbContext --output-dir Migrations/Contracts
         ...

4.  Now go and follow the instructions in the _Uow.Storage.Migrator_ project.

NB) Run `dotnet tool install --global dotnet-ef` to install the EF Core CLI
NB) If replacing migrations, be aware that they can be edited after being generated (eg to add views)
