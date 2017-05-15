//------------------------------------------------------------------------------
// TransactionTagConfiguration.cs
//
// Implementation of: TransactionTagConfiguration (Class) <<entity-configuration>>
// Generated with Domion-MDA - www.coderepo.blog
//------------------------------------------------------------------------------

using Demo.Transactions.Core.Model;
using Domion.Lib.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Transactions.Lib.Data
{
	public class TransactionTagConfiguration : EntityTypeConfiguration<TransactionTag>
	{
		public override void Map(EntityTypeBuilder<TransactionTag> builder)
		{
			builder.ToTable("TransactionTags", schema: "Transactions");

			builder.HasKey(tt => tt.Id);

			builder.Property(tt => tt.RowVersion)
				.IsRowVersion();

			builder.HasOne<BankTransaction>(tt => tt.BankTransaction)
				.WithMany(bt => bt.Tags)
				.HasForeignKey(tt => tt.BankTransaction_Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne<Tag>(tt => tt.Tag)
				.WithMany()
				.HasForeignKey(tt => tt.Tag_Id)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}