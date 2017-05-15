//------------------------------------------------------------------------------
// BankTransactionConfiguration.cs
//
// Implementation of: BankTransactionConfiguration (Class) <<entity-configuration>>
// Generated with Domion-MDA - www.coderepo.blog
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
	public class BankTransactionConfiguration : EntityTypeConfiguration<BankTransaction>
	{
		public override void Map(EntityTypeBuilder<BankTransaction> builder)
		{
			builder.ToTable("BankTransactions", schema: "Transactions");

			builder.HasKey(bt => bt.Id);

			builder.Property(bt => bt.RowVersion)
				.IsRowVersion();

			builder.HasOne<BankAccount>(bt => bt.BankAccount)
				.WithMany(ba => ba.Transactions)
				.HasForeignKey(bt => bt.BankAccount_Id)
				.OnDelete(DeleteBehavior.Cascade);

			// External etities

			builder.HasOne<BudgetClass>(bt => bt.BudgetClass)
				.WithMany()
				.HasForeignKey(bt => bt.BudgetClass_Id)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}