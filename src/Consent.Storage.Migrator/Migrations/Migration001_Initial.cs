﻿using SimpleMigrations;

namespace Consent.Storage.Migrator.Migrations;

[Migration(1, "Initial")]
internal class Migration001_Initial : Migration
{
    protected override void Up()
    {
        Execute(@"
IF SCHEMA_ID(N'users') IS NULL EXEC(N'CREATE SCHEMA [users];');

CREATE TABLE [users].[Users] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);




IF SCHEMA_ID(N'workspaces') IS NULL EXEC(N'CREATE SCHEMA [workspaces];');

CREATE TABLE [workspaces].[Workspaces] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Workspaces] PRIMARY KEY ([Id])
);

CREATE TABLE [workspaces].[Membership] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [Permissions] nvarchar(max) NOT NULL,
    [WorkspaceId] int NOT NULL,
    CONSTRAINT [PK_Membership] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Membership_Workspaces_WorkspaceId] FOREIGN KEY ([WorkspaceId]) REFERENCES [workspaces].[Workspaces] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_Membership_WorkspaceId] ON [workspaces].[Membership] ([WorkspaceId]);




IF SCHEMA_ID(N'purposes') IS NULL EXEC(N'CREATE SCHEMA [purposes];');

CREATE TABLE [purposes].[Purposes] (
    [Id] int NOT NULL IDENTITY,
    [WorkspaceId] int NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Purposes] PRIMARY KEY ([Id])
);




IF SCHEMA_ID(N'contracts') IS NULL EXEC(N'CREATE SCHEMA [contracts];');

CREATE TABLE [contracts].[Contracts] (
    [Id] int NOT NULL IDENTITY,
    [WorkspaceId] int NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Contracts] PRIMARY KEY ([Id])
);

CREATE TABLE [contracts].[ContractVersion] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Text] nvarchar(max) NOT NULL,
    [Status] int NOT NULL,
    [ContractId] int NOT NULL,
    CONSTRAINT [PK_ContractVersion] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ContractVersion_Contracts_ContractId] FOREIGN KEY ([ContractId]) REFERENCES [contracts].[Contracts] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [contracts].[Provision] (
    [Id] int NOT NULL IDENTITY,
    [ContractVersionId] int NOT NULL,
    [Text] nvarchar(max) NOT NULL,
    [PurposeIds] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Provision] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Provision_ContractVersion_ContractVersionId] FOREIGN KEY ([ContractVersionId]) REFERENCES [contracts].[ContractVersion] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_ContractVersion_ContractId] ON [contracts].[ContractVersion] ([ContractId]);

CREATE INDEX [IX_Provision_ContractVersionId] ON [contracts].[Provision] ([ContractVersionId]);
");

        Execute(@"
CREATE VIEW [users].[WorkspaceMembership] AS
SELECT [Id], [WorkspaceId], [Permissions], [UserId]
FROM [workspaces].[Membership]
");
    }

    protected override void Down()
    {
        Execute(@"
DROP VIEW [users].[WorkspaceMembership]

DROP TABLE [users].[Users];

DROP SCHEMA [users]




DROP TABLE [workspaces].[Membership];

DROP TABLE [workspaces].[Workspaces];

DROP SCHEMA [workspaces] 




DROP TABLE [purposes].[Purposes];

DROP SCHEMA [purposes]




DROP TABLE [contracts].[Provision];

DROP TABLE [contracts].[ContractVersion];

DROP TABLE [contracts].[Contracts];

DROP SCHEMA [contracts]
");
    }
}
