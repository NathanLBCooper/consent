﻿How to create these Migrations:

1) Make your changes to the code (to the DBContext)

2) Call `dotnet ef` to generate the EF Migrations for each of the Contexts

        dotnet ef migrations add NameOfTheNewMigration --context UserDbContext --output-dir Migrations/Users
        dotnet ef migrations add NameOfTheNewMigration --context WorkspaceDbContext --output-dir Migrations/Workspaces
        dotnet ef migrations add NameOfTheNewMigration --context ContractDbContext --output-dir Migrations/Contracts

3) Now go and follow the instructions in the *Uow.Storage.Migrator* project.