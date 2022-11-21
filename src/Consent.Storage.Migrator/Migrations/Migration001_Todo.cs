using SimpleMigrations;

namespace Consent.Storage.Migrator.Migrations
{
    [Migration(1, "CreateOrganizationTable")]
    internal class Migration001_Todo : Migration
    {
        protected override void Up()
        {
            Execute(@"
create table [dbo].[Organization](
    [Id] [int] identity primary key,
    [Name] [nvarchar] not null,
    );");
        }

        protected override void Down()
        {
            Execute(@"
drop table [dbo].[Entity];");
        }
    }
}
