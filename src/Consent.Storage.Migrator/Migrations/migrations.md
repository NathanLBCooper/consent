How to create these from EF Core Migrations:

1)  In *Consent.Storage*

        \src\Consent.Storage\

    Call `dotnet ef` to generate SQL from the EF Migrations.

        # Generate Up
        dotnet ef migrations script PreviousMigration NewMigration --context UserDbContext
        dotnet ef migrations script PreviousMigration NewMigration --context WorkspaceDbContext
        dotnet ef migrations script PreviousMigration NewMigration --context PurposeDbContext
        dotnet ef migrations script PreviousMigration NewMigration --context ContractDbContext
        ...

        # Generate Down
        dotnet ef migrations script NewMigration PreviousMigration --context UserDbContext
        dotnet ef migrations script NewMigration PreviousMigration --context WorkspaceDbContext
        dotnet ef migrations script NewMigration PreviousMigration --context PurposeDbContext
        dotnet ef migrations script NewMigration PreviousMigration --context ContractDbContext
        ...

2) Create a new migration in this project.
    Copy in the SQL from the previous step.
    But discard any changes to `[__EFMigrationsHistory]`. This project doesn't use EF Migrations in the database, only to generate SQL for *Simple.Migrations*.

NB) The name for the 0th migration is `0`
