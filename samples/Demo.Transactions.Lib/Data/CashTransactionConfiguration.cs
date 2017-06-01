//------------------------------------------------------------------------------
// CashTransactionConfiguration.cs
//
// Implementation of: CashTransactionConfiguration (Class) <<entity-configuration>>
// Generated with Domion-MDA - http://www.coderepo.blog/domion
//------------------------------------------------------------------------------

using Demo.Budget.Core.Model;
using Demo.Budget.Lib.Data;
using Demo.Transactions.Core.Model;
using Domion.Lib.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Transactions.Lib.Data
{
    public class CashTransactionConfiguration : EntityTypeConfiguration<CashTransaction>
    {
        public override void Map(EntityTypeBuilder<CashTransaction> builder)
        {
            builder.ToTable("CashTransactions", schema: "Transactions");

            builder.HasKey(ct => ct.Id);

            builder.Property(ct => ct.RowVersion)
                .IsRowVersion();

            // External etities

            builder.HasOne<BudgetClass>(ct => ct.BudgetClass)
                .WithMany()
                .HasForeignKey(ct => ct.BudgetClass_Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
