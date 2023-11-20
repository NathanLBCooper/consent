using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Consent.Storage.Migrations.Workspaces;

/// <inheritdoc />
public partial class Initital : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.EnsureSchema(
            name: "workspaces");

        _ = migrationBuilder.CreateTable(
            name: "Workspaces",
            schema: "workspaces",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_Workspaces", x => x.Id);
            });

        _ = migrationBuilder.CreateTable(
            name: "Membership",
            schema: "workspaces",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                UserId = table.Column<int>(type: "int", nullable: false),
                Permissions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                WorkspaceId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_Membership", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_Membership_Workspaces_WorkspaceId",
                    column: x => x.WorkspaceId,
                    principalSchema: "workspaces",
                    principalTable: "Workspaces",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateIndex(
            name: "IX_Membership_WorkspaceId",
            schema: "workspaces",
            table: "Membership",
            column: "WorkspaceId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropTable(
            name: "Membership",
            schema: "workspaces");

        _ = migrationBuilder.DropTable(
            name: "Workspaces",
            schema: "workspaces");
    }
}
