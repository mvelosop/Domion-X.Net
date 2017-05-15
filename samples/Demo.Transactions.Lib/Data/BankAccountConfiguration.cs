//------------------------------------------------------------------------------
// BankAccountConfiguration.cs
//
// Implementation of: BankAccountConfiguration (Class) <<entity-configuration>>
// Generated with Domion-MDA - www.coderepo.blog
//------------------------------------------------------------------------------

using Demo.Transactions.Core.Model;
using Domion.Lib.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Transactions.Lib.Data
{
	public class BankAccountConfiguration : EntityTypeConfiguration<BankAccount>
	{
		public override void Map(EntityTypeBuilder<BankAccount> builder)
		{
			builder.ToTable("BankAccounts", schema: "Transactions");

			builder.HasKey(ba => ba.Id);

			builder.Property(ba => ba.RowVersion)
				.IsRowVersion();
		}
	}
}