//------------------------------------------------------------------------------
// BankTransaction.cs
//
// Implementation of: BankTransaction (Class) <<ef-entity>>
// Generated with Domion-MDA - www.coderepo.blog
//------------------------------------------------------------------------------

using Demo.Budget.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Demo.Transactions.Core.Model
{
	public class BankTransaction
	{
		public BankTransaction()
		{
			Tags = new HashSet<TransactionTag>();
		}

		public virtual decimal Amount { get; set; }

		public virtual BankAccount BankAccount { get; set; }

		public virtual int BankAccount_Id { get; set; }

		[MaxLength(250)]
		public virtual string BankNotes { get; set; }

		public virtual BudgetClass BudgetClass { get; set; }

		public virtual int? BudgetClass_Id { get; set; }

		[Required]
		[MaxLength(3)]
		public virtual string Currency { get; set; }

		public virtual decimal CurrentBalance { get; private set; }

		public virtual DateTime Date { get; set; }

		[Required]
		[MaxLength(250)] // Default string length
		public virtual string Description { get; set; }

		public int Id { get; set; }

		public virtual bool IsCashWithdrawal { get; set; }

		[MaxLength(1000)]
		public virtual string Notes { get; set; }

		public virtual int Number { get; private set; }

		[Required]
		[MaxLength(50)]
		public virtual string Reference { get; set; }

		public virtual Byte[] RowVersion { get; set; }

		public virtual ICollection<TransactionTag> Tags { get; set; }

		[Required]
		[MaxLength(50)]
		public virtual string Type { get; set; }

		public virtual DateTime ValueDate { get; set; }
	}
}