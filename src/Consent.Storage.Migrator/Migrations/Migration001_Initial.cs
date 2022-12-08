using SimpleMigrations;

namespace Consent.Storage.Migrator.Migrations;

[Migration(1, "Initial")]
internal class Migration001_Initial : Migration
{
    protected override void Up()
    {
        Execute(@"
create table [dbo].[User](
    [Id] int identity primary key,
    [Name] nvarchar(max) not null,
    );

create table [dbo].[Workspace](
    [Id] int identity primary key,
    [Name] nvarchar(max) not null
    );

create table [dbo].[UserWorkspaceMembership](
    [UserId] int foreign key references [User]([Id]),
    [WorkspaceId] int foreign key references [Workspace]([Id]),
    [Permission] int not null,
    unique nonclustered ([UserId],[WorkspaceId],[Permission]) 
    );
");
    }

    protected override void Down()
    {
        Execute(@"
drop table [dbo].[User];
drop table [dbo].[Workspace];
drop table [dbo].[UserWorkspaceMembership];
");
    }
}
