using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Consent.Storage.Migrations.Workspaces
{
    /// <inheritdoc />
    public partial class Initital : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "workspaces");

            migrationBuilder.CreateTable(
                name: "Workspaces",
                schema: "workspaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workspaces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Membership",
                schema: "workspaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    User = table.Column<int>(type: "int", nullable: false),
                    WorkspaceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Membership", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Membership_Workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalSchema: "workspaces",
                        principalTable: "Workspaces",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Membership_WorkspaceId",
                schema: "workspaces",
                table: "Membership",
                column: "WorkspaceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Membership",
                schema: "workspaces");

            migrationBuilder.DropTable(
                name: "Workspaces",
                schema: "workspaces");
        }
    }
}
