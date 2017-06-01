//------------------------------------------------------------------------------
// CashTransaction.cs
//
// Implementation of: CashTransaction (Class) <<ef-entity>>
// Generated with Domion-MDA - http://www.coderepo.blog/domion
//------------------------------------------------------------------------------

using Demo.Budget.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Demo.Transactions.Core.Model
{
    public class CashTransaction
    {
        public CashTransaction()
        {
        }

        public virtual decimal Amount { get; private set; }

        public virtual BudgetClass BudgetClass { get; set; }

        public virtual int? BudgetClass_Id { get; set; }

        [Required]
        [MaxLength(250)] // Default string length
        public virtual string Currency { get; private set; }

        public virtual DateTime Date { get; private set; }

        [Required]
        [MaxLength(250)] // Default string length
        public virtual string Description { get; private set; }

        public int Id { get; set; }

        [MaxLength(1000)]
        public virtual string Notes { get; set; }

        public virtual Byte[] RowVersion { get; set; }
    }
}
