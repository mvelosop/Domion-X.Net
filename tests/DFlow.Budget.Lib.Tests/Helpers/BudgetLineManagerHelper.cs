using DFlow.Budget.Core.Model;
using DFlow.Budget.Lib.Services;
using FluentAssertions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DFlow.Budget.Lib.Tests.Helpers
{
    /// <summary>
    /// Test helper class for BudgetLineManager
    ///
    /// Takes a BudgetDbSetup to execute CRUD methods and dispose properly the DbContext.
    /// Manages entity class BudgetLine using data class BudgetLineData as input
    /// </summary>
    public class BudgetLineManagerHelper
    {
        /// <summary>
        /// Creates the test helper for BudgetLineManager
        /// </summary>
        /// <param name="dbSetup"></param>
        public BudgetLineManagerHelper(BudgetDbSetupHelper dbSetup)
        {
            DbSetup = dbSetup;
        }

        public BudgetDbSetupHelper DbSetup { get; set; }

        /// <summary>
        /// Asserts that TryDelete method does not return any error message
        /// </summary>
        /// <param name="entity"></param>
        public void AssertDelete(BudgetLine entity)
        {
            var errors = TryDelete(entity);

            errors.Should().BeEmpty();
        }

        /// <summary>
        /// Asserts that the entities do not exist in the database or are successfully removed
        /// </summary>
        /// <param name="dataArray">Data for the entities to be searched and removed</param>
        public void AssertEntitiesDoNotExist(params BudgetLineData[] dataArray)
        {
            using (var dbContext = DbSetup.GetDbContext())
            {
                var manager = new BudgetLineManager(dbContext);

                foreach (var data in dataArray)
                {
                    var entities = manager.Query(data.KeyExpression).ToList();

                    foreach (var entity in entities)
                    {
                        var errors = manager.TryDelete(entity);

                        errors.Should().BeEmpty();
                    }
                }

                manager.SaveChanges();
            }

            using (var dbContext = DbSetup.GetDbContext())
            {
                var manager = new BudgetLineManager(dbContext);

                foreach (var data in dataArray)
                {
                    var entities = manager.Query(data.KeyExpression).ToList();

                    entities.Should().BeEmpty();
                }
            }
        }

        /// <summary>
        /// Asserts that TryInsert method does not return any error message
        /// </summary>
        /// <param name="entity"></param>
        public void AssertInsert(BudgetLine entity)
        {
            var errors = TryInsert(entity);

            errors.Should().BeEmpty();
        }

        /// <summary>
        /// Asserts that TryUpdate method does not return any error message
        /// </summary>
        /// <param name="entity"></param>
        public void AssertUpdate(BudgetLine entity)
        {
            var errors = TryUpdate(entity);

            errors.Should().BeEmpty();
        }

        /// <summary>
        /// Gets the entity using the KeyDataExpression from the Data class
        /// </summary>
        /// <param name="data">Data for the entity to be searched</param>
        /// <returns>Entity</returns>
        public BudgetLine GetEntity(BudgetLineData data)
        {
            using (var dbContext = DbSetup.GetDbContext())
            {
                var manager = new BudgetLineManager(dbContext);

                return manager.SingleOrDefault(data.KeyExpression);
            }
        }

        /// <summary>
        /// Executes the TryDelete method and returns the validation results or saves the changes if ok.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Validation Results</returns>
        public IEnumerable<ValidationResult> TryDelete(BudgetLine entity)
        {
            using (var dbContext = DbSetup.GetDbContext())
            {
                var manager = new BudgetLineManager(dbContext);

                var errors = manager.TryDelete(entity);

                if (!errors.Any())
                {
                    manager.SaveChanges();
                }

                return errors.ToList();
            }
        }

        /// <summary>
        /// Executes the TryInsert method and returns the validation results or saves the changes if ok.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Validation Results</returns>
        public IEnumerable<ValidationResult> TryInsert(BudgetLine entity)
        {
            using (var dbContext = DbSetup.GetDbContext())
            {
                var manager = new BudgetLineManager(dbContext);

                var errors = manager.TryInsert(entity);

                if (!errors.Any())
                {
                    manager.SaveChanges();
                }

                return errors.ToList();
            }
        }

        /// <summary>
        /// Executes the TryUpdate method and returns the validation results or saves the changes if ok.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Validation Results</returns>
        public IEnumerable<ValidationResult> TryUpdate(BudgetLine entity)
        {
            using (var dbContext = DbSetup.GetDbContext())
            {
                var manager = new BudgetLineManager(dbContext);

                var errors = manager.TryUpdate(entity);

                if (!errors.Any())
                {
                    manager.SaveChanges();
                }

                return errors.ToList();
            }
        }
    }
}
