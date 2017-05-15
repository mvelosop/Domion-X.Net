//------------------------------------------------------------------------------
// BankAccount.cs
//
// Implementation of: BankAccount (Class) <<ef-entity>>
// Generated with Domion-MDA - www.coderepo.blog
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Demo.Transactions.Core.Model
{
	public class BankAccount
	{
		public BankAccount()
		{
			Transactions = new HashSet<BankTransaction>();
		}

		[Required]
		[MaxLength(50)]
		public virtual string AccountNumber { get; set; }

		[Required]
		[MaxLength(250)] // Default string length
		public virtual string BankName { get; set; }

		public virtual decimal CurrentBalance { get; set; }

		public int Id { get; set; }

		public virtual decimal InitialBalance { get; set; }

		public virtual decimal InitialBalanceDate { get; set; }

		public virtual DateTime LastTransactionDate { get; set; }

		public virtual Byte[] RowVersion { get; set; }

		public virtual ICollection<BankTransaction> Transactions { get; set; }

		/// 
		/// <param name="transaction"></param>
		public IEnumerable<ValidationResult> AddTransaction(BankTransaction transaction)
		{
			//TODO: Implementar método: BankAccount.AddTransaction
			return null;
		}

		/// 
		/// <param name="transaction"></param>
		public IEnumerable<ValidationResult> RemoveTransaction(BankTransaction transaction)
		{
			//TODO: Implementar método: BankAccount.RemoveTransaction
			return null;
		}
	}
}