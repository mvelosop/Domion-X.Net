﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DFlow.Budget.Lib.Data;
using DFlow.Budget.Core.Model;

namespace DFlow.Budget.Lib.Migrations
{
    [DbContext(typeof(BudgetDbContext))]
    [Migration("20170607214942_BudgetNameSizeMigration_BudgetDbContext")]
    partial class BudgetNameSizeMigration_BudgetDbContext
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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
                        .HasMaxLength(100);

                    b.Property<int>("Order");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<int>("Tennant_Id");

                    b.Property<int>("TransactionType");

                    b.HasKey("Id");

                    b.HasIndex("Tennant_Id", "Name")
                        .IsUnique();

                    b.ToTable("BudgetClasses","Budget");
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

                    b.HasIndex("BudgetClass_Id", "Name")
                        .IsUnique();

                    b.ToTable("BudgetLines","Budget");
                });

            modelBuilder.Entity("DFlow.Tennants.Core.Model.Tennant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Owner")
                        .HasMaxLength(250);

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("Owner")
                        .IsUnique();

                    b.ToTable("Tennants","Tennants");
                });

            modelBuilder.Entity("DFlow.Budget.Core.Model.BudgetClass", b =>
                {
                    b.HasOne("DFlow.Tennants.Core.Model.Tennant", "Tennant")
                        .WithMany()
                        .HasForeignKey("Tennant_Id");
                });

            modelBuilder.Entity("DFlow.Budget.Core.Model.BudgetLine", b =>
                {
                    b.HasOne("DFlow.Budget.Core.Model.BudgetClass", "BudgetClass")
                        .WithMany("BudgetLines")
                        .HasForeignKey("BudgetClass_Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
