using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DFlow.Tennants.Lib.Migrations
{
    public partial class CreateMigration_TennantsDbContext : Migration
    {
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tennants",
                schema: "Tennants");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Tennants");

            migrationBuilder.CreateTable(
                name: "Tennants",
                schema: "Tennants",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Owner = table.Column<string>(maxLength: 250, nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tennants", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tennants_Owner",
                schema: "Tennants",
                table: "Tennants",
                column: "Owner",
                unique: true);
        }
    }
}
