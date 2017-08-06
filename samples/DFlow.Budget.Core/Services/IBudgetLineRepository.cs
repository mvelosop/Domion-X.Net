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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace DFlow.Budget.Core.Services
{
    public interface IBudgetLineRepository : IRepositoryQuery<BudgetLine>, IEntityFinder<BudgetLine, int>
    {
        /// <summary>
        ///     Returns another BudgetLine with the same Name.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The duplicate BudgetLine or null if none</returns>
        BudgetLine FindDuplicateByName(BudgetLine entity);

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
        ///     <para>
        ///         Refreshes the entity in the DbContext's change tracker, requerying the database.
        ///     </para>
        ///     <para>
        ///         Important, this only refreshes the passed entity. It does not refresh the related entities
        ///         (navigation or collection properties). If needed yo have to modify this method and call the
        ///         method on each one.
        ///     </para>
        /// </summary>
        Task<BudgetLine> RefreshAsync(BudgetLine entity);

        /// <summary>
        ///     Saves changes from the DbContext's change tracker to the database.
        /// </summary>
        void SaveChanges();

        /// <summary>
        ///     Saves changes from the DbContext's change tracker to the database.
        /// </summary>
        Task SaveChangesAsync();

        /// <summary>
        ///     Marks an entity for deletion in the DbContext's change tracker if no errors are found in the ValidateDelete method.
        /// </summary>
        List<ValidationResult> TryDelete(BudgetLine entity);

        /// <summary>
        ///     Adds an entity for insertion in the DbContext's change tracker if no errors are found in the ValidateSave method.
        ///     This method also checks that the concurrency token (RowVersion) is EMPTY.
        /// </summary>
        List<ValidationResult> TryInsert(BudgetLine entity);

        /// <summary>
        ///     Marks an entity for update in the DbContext's change tracker if no errors are found in the ValidateSave method.
        ///     This method also checks that the concurrency token (RowVersion) is NOT EMPTY.
        /// </summary>
        List<ValidationResult> TryUpdate(BudgetLine entity);

        /// <summary>
        ///     Calls TryInsert or TryUpdate accordingly, based on the value of the Id property;
        /// </summary>
        List<ValidationResult> TryUpsert(BudgetLine entity);

        /// <summary>
        ///     Validates if it's ok to delete the entity from the database.
        /// </summary>
        IEnumerable<ValidationResult> ValidateDelete(BudgetLine entity);

        /// <summary>
        ///     Validates if it's ok to save the new or updated entity to the database.
        /// </summary>
        IEnumerable<ValidationResult> ValidateSave(BudgetLine model);
    }
}
