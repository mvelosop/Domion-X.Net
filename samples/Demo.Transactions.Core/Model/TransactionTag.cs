//------------------------------------------------------------------------------
// TransactionTag.cs
//
// Implementation of: TransactionTag (Class) <<ef-entity>>
// Generated with Domion-MDA - www.coderepo.blog
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Demo.Transactions.Core.Model
{
	public class TransactionTag
	{
		public TransactionTag()
		{
		}

		public virtual BankTransaction BankTransaction { get; set; }

		public virtual int BankTransaction_Id { get; set; }

		public int Id { get; set; }

		public virtual Byte[] RowVersion { get; set; }

		public virtual Tag Tag { get; set; }

		public virtual int Tag_Id { get; set; }
	}
}