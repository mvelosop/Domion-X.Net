//------------------------------------------------------------------------------
// BankAccount.cs
//
// Implementation of: BankAccount (Class) <<ef-entity>>
// Generated with Domion-MDA - www.coderepo.blog
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Demo.Transactions.Core.Model
{
    public class BankAccount
    {
        private List<BankTransaction> _transactions = new List<BankTransaction>();

        public BankAccount()
        {
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

        public virtual DateTime? InitialBalanceDate { get; set; }

        public virtual DateTime? LastTransactionDate { get; set; }

        public virtual int LastTransactionNumber { get; set; }

        public virtual Byte[] RowVersion { get; set; }

        public virtual IEnumerable<BankTransaction> Transactions => new ReadOnlyCollection<BankTransaction>(_transactions);

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
