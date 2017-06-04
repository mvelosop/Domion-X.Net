using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DFlow.Transactions.Lib.Migrations
{
    public partial class CreateMigration_TransactionsDbContext : Migration
    {
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CashTransactions",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "TransactionTags",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "BankTransactions",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "Tags",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "BankAccounts",
                schema: "Transactions");

            //migrationBuilder.DropTable(
            //    name: "BudgetLines",
            //    schema: "Budget");

            migrationBuilder.DropTable(
                name: "BudgetClass");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.EnsureSchema(
            //    name: "Budget");

            migrationBuilder.EnsureSchema(
                name: "Transactions");

            migrationBuilder.CreateTable(
                name: "BudgetClass",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Order = table.Column<int>(nullable: false),
                    RowVersion = table.Column<byte[]>(nullable: true),
                    TransactionType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetClass", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankAccounts",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountName = table.Column<string>(maxLength: 250, nullable: true),
                    AccountNumber = table.Column<string>(maxLength: 50, nullable: false),
                    BankName = table.Column<string>(maxLength: 250, nullable: false),
                    CurrentBalance = table.Column<decimal>(nullable: false),
                    InitialBalance = table.Column<decimal>(nullable: false),
                    InitialBalanceDate = table.Column<DateTime>(nullable: true),
                    LastTransactionDate = table.Column<DateTime>(nullable: true),
                    LastTransactionNumber = table.Column<int>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            //migrationBuilder.CreateTable(
            //    name: "BudgetLines",
            //    schema: "Budget",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Amount = table.Column<decimal>(nullable: false),
            //        BudgetClass_Id = table.Column<int>(nullable: false),
            //        Name = table.Column<string>(maxLength: 100, nullable: false),
            //        Order = table.Column<int>(nullable: false),
            //        RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BudgetLines", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_BudgetLines_BudgetClass_BudgetClass_Id",
            //            column: x => x.BudgetClass_Id,
            //            principalTable: "BudgetClass",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            migrationBuilder.CreateTable(
                name: "BankTransactions",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<decimal>(nullable: false),
                    BankAccount_Id = table.Column<int>(nullable: false),
                    BankNotes = table.Column<string>(maxLength: 250, nullable: true),
                    BudgetLine_Id = table.Column<int>(nullable: true),
                    Currency = table.Column<string>(maxLength: 3, nullable: false),
                    CurrentBalance = table.Column<decimal>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: false),
                    IsCashWithdrawal = table.Column<bool>(nullable: false),
                    Notes = table.Column<string>(maxLength: 1000, nullable: true),
                    Order = table.Column<int>(nullable: false),
                    Reference = table.Column<string>(maxLength: 50, nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Type = table.Column<string>(maxLength: 50, nullable: false),
                    ValueDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankTransactions_BankAccounts_BankAccount_Id",
                        column: x => x.BankAccount_Id,
                        principalSchema: "Transactions",
                        principalTable: "BankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BankTransactions_BudgetLines_BudgetLine_Id",
                        column: x => x.BudgetLine_Id,
                        principalSchema: "Budget",
                        principalTable: "BudgetLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CashTransactions",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<decimal>(nullable: false),
                    BudgetLine_Id = table.Column<int>(nullable: true),
                    Currency = table.Column<string>(maxLength: 250, nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: false),
                    Notes = table.Column<string>(maxLength: 1000, nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashTransactions_BudgetLines_BudgetLine_Id",
                        column: x => x.BudgetLine_Id,
                        principalSchema: "Budget",
                        principalTable: "BudgetLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransactionTags",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BankTransaction_Id = table.Column<int>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Tag_Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionTags_BankTransactions_BankTransaction_Id",
                        column: x => x.BankTransaction_Id,
                        principalSchema: "Transactions",
                        principalTable: "BankTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionTags_Tags_Tag_Id",
                        column: x => x.Tag_Id,
                        principalSchema: "Transactions",
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            //migrationBuilder.CreateIndex(
            //    name: "IX_BudgetLines_BudgetClass_Id",
            //    schema: "Budget",
            //    table: "BudgetLines",
            //    column: "BudgetClass_Id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_BudgetLines_Name",
            //    schema: "Budget",
            //    table: "BudgetLines",
            //    column: "Name",
            //    unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_AccountName",
                schema: "Transactions",
                table: "BankAccounts",
                column: "AccountName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BankTransactions_BankAccount_Id",
                schema: "Transactions",
                table: "BankTransactions",
                column: "BankAccount_Id");

            migrationBuilder.CreateIndex(
                name: "IX_BankTransactions_BudgetLine_Id",
                schema: "Transactions",
                table: "BankTransactions",
                column: "BudgetLine_Id");

            migrationBuilder.CreateIndex(
                name: "IX_CashTransactions_BudgetLine_Id",
                schema: "Transactions",
                table: "CashTransactions",
                column: "BudgetLine_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                schema: "Transactions",
                table: "Tags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTags_BankTransaction_Id",
                schema: "Transactions",
                table: "TransactionTags",
                column: "BankTransaction_Id");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTags_Tag_Id",
                schema: "Transactions",
                table: "TransactionTags",
                column: "Tag_Id");
        }
    }
}
