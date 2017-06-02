using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DFlow.Transactions.Lib.Data;
using DFlow.Budget.Core.Model;

namespace DFlow.Transactions.Lib.Migrations
{
    [DbContext(typeof(TransactionsDbContext))]
    partial class TransactionsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DFlow.Budget.Core.Model.BudgetClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("Order");

                    b.Property<byte[]>("RowVersion");

                    b.Property<int>("TransactionType");

                    b.HasKey("Id");

                    b.ToTable("BudgetClass");
                });

            modelBuilder.Entity("DFlow.Budget.Core.Model.BudgetLine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount");

                    b.Property<int>("BudgetClass_Id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("Order");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("BudgetClass_Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("BudgetLines","Budget");
                });

            modelBuilder.Entity("DFlow.Transactions.Core.Model.BankAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountName")
                        .HasMaxLength(250);

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("BankName")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<decimal>("CurrentBalance");

                    b.Property<decimal>("InitialBalance");

                    b.Property<DateTime?>("InitialBalanceDate");

                    b.Property<DateTime?>("LastTransactionDate");

                    b.Property<int>("LastTransactionNumber");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("AccountName")
                        .IsUnique();

                    b.ToTable("BankAccounts","Transactions");
                });

            modelBuilder.Entity("DFlow.Transactions.Core.Model.BankTransaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount");

                    b.Property<int>("BankAccount_Id");

                    b.Property<string>("BankNotes")
                        .HasMaxLength(250);

                    b.Property<int?>("BudgetLine_Id");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(3);

                    b.Property<decimal>("CurrentBalance");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<bool>("IsCashWithdrawal");

                    b.Property<string>("Notes")
                        .HasMaxLength(1000);

                    b.Property<int>("Order");

                    b.Property<string>("Reference")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("ValueDate");

                    b.HasKey("Id");

                    b.HasIndex("BankAccount_Id");

                    b.HasIndex("BudgetLine_Id");

                    b.ToTable("BankTransactions","Transactions");
                });

            modelBuilder.Entity("DFlow.Transactions.Core.Model.CashTransaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount");

                    b.Property<int?>("BudgetLine_Id");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<string>("Notes")
                        .HasMaxLength(1000);

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("BudgetLine_Id");

                    b.ToTable("CashTransactions","Transactions");
                });

            modelBuilder.Entity("DFlow.Transactions.Core.Model.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Tags","Transactions");
                });

            modelBuilder.Entity("DFlow.Transactions.Core.Model.TransactionTag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BankTransaction_Id");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<int>("Tag_Id");

                    b.HasKey("Id");

                    b.HasIndex("BankTransaction_Id");

                    b.HasIndex("Tag_Id");

                    b.ToTable("TransactionTags","Transactions");
                });

            modelBuilder.Entity("DFlow.Budget.Core.Model.BudgetLine", b =>
                {
                    b.HasOne("DFlow.Budget.Core.Model.BudgetClass", "BudgetClass")
                        .WithMany()
                        .HasForeignKey("BudgetClass_Id");
                });

            modelBuilder.Entity("DFlow.Transactions.Core.Model.BankTransaction", b =>
                {
                    b.HasOne("DFlow.Transactions.Core.Model.BankAccount", "BankAccount")
                        .WithMany("Transactions")
                        .HasForeignKey("BankAccount_Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DFlow.Budget.Core.Model.BudgetLine", "BudgetLine")
                        .WithMany()
                        .HasForeignKey("BudgetLine_Id");
                });

            modelBuilder.Entity("DFlow.Transactions.Core.Model.CashTransaction", b =>
                {
                    b.HasOne("DFlow.Budget.Core.Model.BudgetLine", "BudgetLine")
                        .WithMany()
                        .HasForeignKey("BudgetLine_Id");
                });

            modelBuilder.Entity("DFlow.Transactions.Core.Model.TransactionTag", b =>
                {
                    b.HasOne("DFlow.Transactions.Core.Model.BankTransaction", "BankTransaction")
                        .WithMany("Tags")
                        .HasForeignKey("BankTransaction_Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DFlow.Transactions.Core.Model.Tag", "Tag")
                        .WithMany()
                        .HasForeignKey("Tag_Id");
                });
        }
    }
}
