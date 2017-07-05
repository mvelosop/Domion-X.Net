using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DFlow.Tenants.Lib.Data;

namespace DFlow.Tenants.Lib.Migrations
{
    [DbContext(typeof(TenantsDbContext))]
    [Migration("20170705160922_CreateMigration_TenantsDbContext")]
    partial class CreateMigration_TenantsDbContext
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DFlow.Tenants.Core.Model.Tenant", b =>
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

                    b.ToTable("Tenants","Tenants");
                });
        }
    }
}
