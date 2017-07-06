using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DFlow.Budget.Lib.Migrations
{
    public partial class AddTenantsMigration_BudgetDbContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BudgetClasses_Name",
                schema: "Budget",
                table: "BudgetClasses");

            //migrationBuilder.EnsureSchema(
            //    name: "Tenants");

            migrationBuilder.AddColumn<int>(
                name: "Tenant_Id",
                schema: "Budget",
                table: "BudgetClasses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BudgetLines",
                schema: "Budget",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<decimal>(nullable: false),
                    BudgetClass_Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Order = table.Column<int>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetLines_BudgetClasses_BudgetClass_Id",
                        column: x => x.BudgetClass_Id,
                        principalSchema: "Budget",
                        principalTable: "BudgetClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            //migrationBuilder.CreateTable(
            //    name: "Tenants",
            //    schema: "Tenants",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Owner = table.Column<string>(maxLength: 250, nullable: true),
            //        RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Tenants", x => x.Id);
            //    });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetClasses_Tenant_Id_Name",
                schema: "Budget",
                table: "BudgetClasses",
                columns: new[] { "Tenant_Id", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetLines_BudgetClass_Id",
                schema: "Budget",
                table: "BudgetLines",
                column: "BudgetClass_Id");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetLines_Name",
                schema: "Budget",
                table: "BudgetLines",
                column: "Name",
                unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Tenants_Owner",
            //    schema: "Tenants",
            //    table: "Tenants",
            //    column: "Owner",
            //    unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetClasses_Tenants_Tenant_Id",
                schema: "Budget",
                table: "BudgetClasses",
                column: "Tenant_Id",
                principalSchema: "Tenants",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetClasses_Tenants_Tenant_Id",
                schema: "Budget",
                table: "BudgetClasses");

            migrationBuilder.DropTable(
                name: "BudgetLines",
                schema: "Budget");

            //migrationBuilder.DropTable(
            //    name: "Tenants",
            //    schema: "Tenants");

            migrationBuilder.DropIndex(
                name: "IX_BudgetClasses_Tenant_Id_Name",
                schema: "Budget",
                table: "BudgetClasses");

            migrationBuilder.DropColumn(
                name: "Tenant_Id",
                schema: "Budget",
                table: "BudgetClasses");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetClasses_Name",
                schema: "Budget",
                table: "BudgetClasses",
                column: "Name",
                unique: true);
        }
    }
}
