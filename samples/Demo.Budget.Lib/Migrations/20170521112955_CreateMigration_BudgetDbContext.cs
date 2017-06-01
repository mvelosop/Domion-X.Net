﻿using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Demo.Budget.Lib.Migrations
{
    public partial class CreateMigration_BudgetDbContext : Migration
    {
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetClasses",
                schema: "Budget");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Budget");

            migrationBuilder.CreateTable(
                name: "BudgetClasses",
                schema: "Budget",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    TransactionType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetClasses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetClasses_Name",
                schema: "Budget",
                table: "BudgetClasses",
                column: "Name",
                unique: true);
        }
    }
}
