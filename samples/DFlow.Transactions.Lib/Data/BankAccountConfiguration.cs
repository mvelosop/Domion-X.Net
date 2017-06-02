//------------------------------------------------------------------------------
//  BankAccountConfiguration.cs
//
//  Implementation of: BankAccountConfiguration (Class) <<entity-configuration>>
//  Generated by Domion-MDA - http://www.coderepo.blog/domion
//
//  Created on     : 02-jun-2017 10:49:06
//  Original author: Miguel
//------------------------------------------------------------------------------

using DFlow.Transactions.Core.Model;
using Domion.Lib.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DFlow.Transactions.Lib.Data
{
	public class BankAccountConfiguration : EntityTypeConfiguration<BankAccount>
	{
		public override void Map(EntityTypeBuilder<BankAccount> builder)
		{
			builder.ToTable("BankAccounts", schema: "Transactions");

			builder.HasKey(ba => ba.Id);

			builder.Property(ba => ba.RowVersion)
				.IsRowVersion();

			builder.HasIndex(ba => ba.AccountName)
				.IsUnique();
		}
	}
}