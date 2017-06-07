using Microsoft.EntityFrameworkCore.Migrations;

namespace DFlow.Budget.Lib.Migrations
{
    public partial class BudgetNameSizeMigration_BudgetDbContext : Migration
    {
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Budget",
                table: "BudgetClasses",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100);
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Budget",
                table: "BudgetClasses",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50);
        }
    }
}
