//------------------------------------------------------------------------------
//  IBudgetLineRepository.cs
//
//  Implementation of: IBudgetLineRepository (Interface) <<entity-repository>>
//  Generated by Domion-MDA - http://www.coderepo.blog/domion
//
//  Created on     : 04-jul-2017 23:57:09
//  Original author: Miguel
//------------------------------------------------------------------------------

using DFlow.Budget.Core.Model;
using Domion.Core.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace DFlow.Budget.Core.Services
{
	public interface IBudgetLineRepository : IRepositoryQuery<BudgetLine>, IEntityFinder<BudgetLine, int>
	{
		/// <summary>
		///     <para>
		///         Refreshes the entity in the DbContext's change tracker, requerying the database.
		///     </para>
		///     <para>
		///         Important, this only refreshes the passed entity. It does not refresh the related entities 
		///         (navigation or collection properties). If needed yo have to modify this method and call the
		///         method on each one.
		///     </para>
		/// </summary>
		BudgetLine Refresh(BudgetLine entity);

		/// <summary>
		///     Saves changes from the DbContext's change tracker to the database.
		/// </summary>
		void SaveChanges();

		/// <summary>
		///     Marks an entity for deletion in the DbContext's change tracker if no errors are found in the ValidateDelete method.
		/// </summary>
		IEnumerable<ValidationResult> TryDelete(BudgetLine entity);

		/// <summary>
		///     Adds an entity for insertion in the DbContext's change tracker if no errors are found in the ValidateSave method. 
		///     This method also checks that the concurrency token (RowVersion) is EMPTY.
		/// </summary>
		IEnumerable<ValidationResult> TryInsert(BudgetLine entity);

		/// <summary>
		///     Marks an entity for update in the DbContext's change tracker if no errors are found in the ValidateSave method.
		///     This method also checks that the concurrency token (RowVersion) is NOT EMPTY.
		/// </summary>
		IEnumerable<ValidationResult> TryUpdate(BudgetLine entity);

		/// <summary>
		///     Calls TryInsert or TryUpdate accordingly, based on the value of the Id property;
		/// </summary>
		IEnumerable<ValidationResult> TryUpsert(BudgetLine entity);
	}
}