﻿using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DFlow.Budget.Lib.Migrations
{
    public partial class AddTennantMigration_BudgetDbContext : Migration
    {
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetClasses_Tennants_Tennant_Id",
                schema: "Budget",
                table: "BudgetClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetLines_BudgetClasses_BudgetClass_Id",
                schema: "Budget",
                table: "BudgetLines");

            //migrationBuilder.DropTable(
            //    name: "Tennants",
            //    schema: "Tennants");

            migrationBuilder.DropIndex(
                name: "IX_BudgetLines_BudgetClass_Id_Name",
                schema: "Budget",
                table: "BudgetLines");

            migrationBuilder.DropIndex(
                name: "IX_BudgetClasses_Tennant_Id_Name",
                schema: "Budget",
                table: "BudgetClasses");

            migrationBuilder.DropColumn(
                name: "Tennant_Id",
                schema: "Budget",
                table: "BudgetClasses");

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

            migrationBuilder.CreateIndex(
                name: "IX_BudgetClasses_Name",
                schema: "Budget",
                table: "BudgetClasses",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetLines_BudgetClasses_BudgetClass_Id",
                schema: "Budget",
                table: "BudgetLines",
                column: "BudgetClass_Id",
                principalSchema: "Budget",
                principalTable: "BudgetClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetLines_BudgetClasses_BudgetClass_Id",
                schema: "Budget",
                table: "BudgetLines");

            migrationBuilder.DropIndex(
                name: "IX_BudgetLines_BudgetClass_Id",
                schema: "Budget",
                table: "BudgetLines");

            migrationBuilder.DropIndex(
                name: "IX_BudgetLines_Name",
                schema: "Budget",
                table: "BudgetLines");

            migrationBuilder.DropIndex(
                name: "IX_BudgetClasses_Name",
                schema: "Budget",
                table: "BudgetClasses");

            //migrationBuilder.EnsureSchema(
            //    name: "Tennants");

            migrationBuilder.AddColumn<int>(
                name: "Tennant_Id",
                schema: "Budget",
                table: "BudgetClasses",
                nullable: false,
                defaultValue: 0);

            //migrationBuilder.CreateTable(
            //    name: "Tennants",
            //    schema: "Tennants",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Owner = table.Column<string>(maxLength: 250, nullable: true),
            //        RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Tennants", x => x.Id);
            //    });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetLines_BudgetClass_Id_Name",
                schema: "Budget",
                table: "BudgetLines",
                columns: new[] { "BudgetClass_Id", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetClasses_Tennant_Id_Name",
                schema: "Budget",
                table: "BudgetClasses",
                columns: new[] { "Tennant_Id", "Name" },
                unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Tennants_Owner",
            //    schema: "Tennants",
            //    table: "Tennants",
            //    column: "Owner",
            //    unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetClasses_Tennants_Tennant_Id",
                schema: "Budget",
                table: "BudgetClasses",
                column: "Tennant_Id",
                principalSchema: "Tennants",
                principalTable: "Tennants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetLines_BudgetClasses_BudgetClass_Id",
                schema: "Budget",
                table: "BudgetLines",
                column: "BudgetClass_Id",
                principalSchema: "Budget",
                principalTable: "BudgetClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}