﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DFlow.Tennants.Lib.Data;

namespace DFlow.Tennants.Lib.Migrations
{
    [DbContext(typeof(TennantsDbContext))]
    [Migration("20170605203824_CreateMigration_TennantsDbContext")]
    partial class CreateMigration_TennantsDbContext
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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
        }
    }
}