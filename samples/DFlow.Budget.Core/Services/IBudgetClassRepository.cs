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
using System.Threading.Tasks;

namespace DFlow.Budget.Core.Services
{
    public interface IBudgetClassRepository : IRepositoryQuery<BudgetClass>, IEntityFinder<BudgetClass, int>
    {
        /// <summary>
        ///     Returns another BudgetClass with the same Name.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The duplicate BudgetClass or null if none</returns>
        BudgetClass FindDuplicateByName(BudgetClass entity);

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
        ///     <para>
        ///         Refreshes the entity in the DbContext's change tracker, requerying the database.
        ///     </para>
        ///     <para>
        ///         Important, this only refreshes the passed entity. It does not refresh the related entities 
        ///         (navigation or collection properties). If needed yo have to modify this method and call the
        ///         method on each one.
        ///     </para>
        /// </summary>
        Task<BudgetClass> RefreshAsync(BudgetClass entity);

        /// <summary>
        ///     Saves changes from the DbContext's change tracker to the database.
        /// </summary>
        int SaveChanges();

        /// <summary>
        ///     Saves changes from the DbContext's change tracker to the database.
        /// </summary>
        Task<int> SaveChangesAsync();

        /// <summary>
        ///     Marks an entity for deletion in the DbContext's change tracker if no errors are found in the ValidateDelete method.
        /// </summary>
        List<ValidationResult> TryDelete(BudgetClass entity);

        /// <summary>
        ///     Adds an entity for insertion in the DbContext's change tracker if no errors are found in the ValidateSave method.
        ///     This method also checks that the concurrency token (RowVersion) is EMPTY.
        /// </summary>
        List<ValidationResult> TryInsert(BudgetClass entity);

        /// <summary>
        ///     Marks an entity for update in the DbContext's change tracker if no errors are found in the ValidateSave method.
        ///     This method also checks that the concurrency token (RowVersion) is NOT EMPTY.
        /// </summary>
        List<ValidationResult> TryUpdate(BudgetClass entity);

        /// <summary>
        ///     Calls TryInsert or TryUpdate accordingly, based on the value of the Id property;
        /// </summary>
        List<ValidationResult> TryUpsert(BudgetClass entity);

        /// <summary>
        ///     Validates if it's ok to delete the entity from the database.
        /// </summary>
        IEnumerable<ValidationResult> ValidateDelete(BudgetClass entity);

        /// <summary>
        ///     Validates if it's ok to save the new or updated entity to the database.
        /// </summary>
        IEnumerable<ValidationResult> ValidateSave(BudgetClass model);
    }
}
