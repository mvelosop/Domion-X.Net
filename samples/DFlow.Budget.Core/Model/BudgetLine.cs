//------------------------------------------------------------------------------
//  BudgetLine.cs
//
//  Implementation of: BudgetLine (Class) <<ef-entity>>
//  Generated by Domion-MDA - http://www.coderepo.blog/domion
//
//  Created on     : 02-jun-2017 10:49:08
//  Original author: Miguel
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DFlow.Budget.Core.Model
{
	public class BudgetLine
	{
		public BudgetLine()
		{
		}

		public virtual decimal Amount { get; set; }

		public virtual BudgetClass BudgetClass { get; set; }

		public virtual int BudgetClass_Id { get; set; }

		public int Id { get; set; }

		[Required]
		[MaxLength(100)]
		public virtual string Name { get; set; } // Key data ----------

		public virtual int Order { get; set; }

		public virtual Byte[] RowVersion { get; set; }
	}
}