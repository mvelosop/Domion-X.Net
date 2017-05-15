//------------------------------------------------------------------------------
// IBudgetClassManager.cs
//
// Implementation of: IBudgetClassManager (Interface) <<entity-manager>>
// Generated with Domion-MDA - www.coderepo.blog
//------------------------------------------------------------------------------

using Demo.Budget.Core.Model;
using Domion.Core.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace Demo.Budget.Core.Services
{
	public interface IBudgetClassManager : IEntityManager<BudgetClass, int>
	{
		BudgetClass Refresh(BudgetClass entity);

		void SaveChanges();

		IEnumerable<ValidationResult> TryDelete(BudgetClass entity);

		IEnumerable<ValidationResult> TryInsert(BudgetClass entity);

		IEnumerable<ValidationResult> TryUpdate(BudgetClass entity);

		IEnumerable<ValidationResult> TryUpsert(BudgetClass entity);

		IEnumerable<ValidationResult> ValidateDelete(BudgetClass entity);

		IEnumerable<ValidationResult> ValidateSave(BudgetClass entity);
	}
}