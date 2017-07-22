//------------------------------------------------------------------------------
//  IBudgetClassRepository.cs
//
//  Implementation of: IBudgetClassRepository (Interface) <<entity-repository>>
//  Generated by Domion-MDA - http://www.coderepo.blog/domion
//
//  Created on     : 02-jun-2017 10:49:09
//  Original author: Miguel
//------------------------------------------------------------------------------

using DFlow.Budget.Core.Model;
using Domion.Core.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DFlow.Budget.Core.Services
{
    public interface IBudgetClassRepository : IRepositoryQuery<BudgetClass>, IEntityFinder<BudgetClass, int>
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
        BudgetClass Refresh(BudgetClass entity);

        /// <summary>
        ///     Saves changes from the DbContext's change tracker to the database.
        /// </summary>
        void SaveChanges();

        /// <summary>
        ///     Marks an entity for deletion in the DbContext's change tracker if no errors are found in the ValidateDelete method.
        /// </summary>
        IEnumerable<ValidationResult> TryDelete(BudgetClass entity);

        /// <summary>
        ///     Adds an entity for insertion in the DbContext's change tracker if no errors are found in the ValidateSave method.
        ///     This method also checks that the concurrency token (RowVersion) is EMPTY.
        /// </summary>
        IEnumerable<ValidationResult> TryInsert(BudgetClass entity);

        /// <summary>
        ///     Marks an entity for update in the DbContext's change tracker if no errors are found in the ValidateSave method.
        ///     This method also checks that the concurrency token (RowVersion) is NOT EMPTY.
        /// </summary>
        IEnumerable<ValidationResult> TryUpdate(BudgetClass entity);

        /// <summary>
        ///     Calls TryInsert or TryUpdate accordingly, based on the value of the Id property;
        /// </summary>
        IEnumerable<ValidationResult> TryUpsert(BudgetClass entity);

        /// <summary>
        ///     Returns the validation results for conditions that prevent the entity to be removed.
        /// </summary>
        IEnumerable<ValidationResult> ValidateDelete(BudgetClass entity);

        /// <summary>
        ///     Returns the validation results for conditions that prevent the entity to be added or updated.
        /// </summary>
        IEnumerable<ValidationResult> ValidateSave(BudgetClass entity);
    }
}
