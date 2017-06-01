//------------------------------------------------------------------------------
// BudgetClass.cs
//
// Implementation of: BudgetClass (Class) <<ef-entity>>
// Generated with Domion-MDA - http://www.coderepo.blog/domion
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Demo.Budget.Core.Model
{
    public class BudgetClass
    {
        public BudgetClass()
        {
            TransactionType = TransactionType.Income;
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public virtual string Name { get; set; }

        public virtual Byte[] RowVersion { get; set; }

        public virtual TransactionType TransactionType { get; set; }
    }
}
