using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Consent.Storage.Migrations.Contracts
{
    /// <inheritdoc />
    public partial class Initital : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.EnsureSchema(
                name: "contracts");

            _ = migrationBuilder.CreateTable(
                name: "Contracts",
                schema: "contracts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Contracts", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "ContractVersion",
                schema: "contracts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ContractId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_ContractVersion", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_ContractVersion_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalSchema: "contracts",
                        principalTable: "Contracts",
                        principalColumn: "Id");
                });

            _ = migrationBuilder.CreateTable(
                name: "Provision",
                schema: "contracts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Permissions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContractVersionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Provision", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_Provision_ContractVersion_ContractVersionId",
                        column: x => x.ContractVersionId,
                        principalSchema: "contracts",
                        principalTable: "ContractVersion",
                        principalColumn: "Id");
                });

            _ = migrationBuilder.CreateIndex(
                name: "IX_ContractVersion_ContractId",
                schema: "contracts",
                table: "ContractVersion",
                column: "ContractId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Provision_ContractVersionId",
                schema: "contracts",
                table: "Provision",
                column: "ContractVersionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "Provision",
                schema: "contracts");

            _ = migrationBuilder.DropTable(
                name: "ContractVersion",
                schema: "contracts");

            _ = migrationBuilder.DropTable(
                name: "Contracts",
                schema: "contracts");
        }
    }
}
