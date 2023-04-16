using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Consent.Storage.Migrations.Users;

/// <inheritdoc />
public partial class Initital : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.EnsureSchema(
            name: "users");

        _ = migrationBuilder.CreateTable(
            name: "Users",
            schema: "users",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_Users", x => x.Id);
            });

        _ = migrationBuilder.Sql(@"
CREATE VIEW [users].[WorkspaceMembership] AS
SELECT [Id], [WorkspaceId], [Permissions], [UserId]
FROM [workspaces].[Membership]
");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropTable(
            name: "Users",
            schema: "users");

        _ = migrationBuilder.Sql(@"
DROP VIEW [users].[WorkspaceMembership]
");
    }
}
