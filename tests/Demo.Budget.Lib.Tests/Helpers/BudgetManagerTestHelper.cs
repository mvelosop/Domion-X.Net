using Demo.Budget.Core.Model;
using Demo.Budget.Lib.Services;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Demo.Budget.Lib.Tests.Helpers
{
    /// <summary>
    /// Test helper class for BudgetClassManager
    /// 
    /// Takes a BudgetDbSetup to execute CRUD methods and dispose properly the DbContext.
    /// Manages entity class BudgetClass using data class BudgetClassData as input
    /// </summary>
    public class BudgetManagerTestHelper
    {
        /// <summary>
        /// Creates the test helper for BudgetManager
        /// </summary>
        /// <param name="dbSetup"></param>
        public BudgetManagerTestHelper(BudgetDbSetup dbSetup)
        {
            DbSetup = dbSetup;
        }

        public BudgetDbSetup DbSetup { get; set; }

        /// <summary>
        /// Asserts that TryDelete method does not return any error message
        /// </summary>
        /// <param name="entity"></param>
        public void AssertDelete(BudgetClass entity)
        {
            var errors = TryDelete(entity);

            errors.Should().BeEmpty();
        }

        /// <summary>
        /// Asserts that the entities do not exist in the database or are succesfully removed
        /// </summary>
        /// <param name="dataArray">Data for the entities to be searched and removed</param>
        public void AssertEntitiesDoNotExist(params BudgetClassData[] dataArray)
        {
            using (var dbContext = DbSetup.GetDbContext())
            {
                var manager = new BudgetClassManager(dbContext);

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
                var manager = new BudgetClassManager(dbContext);

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
        public void AssertInsert(BudgetClass entity)
        {
            var errors = TryInsert(entity);

            errors.Should().BeEmpty();
        }

        /// <summary>
        /// Asserts that TryUpdate method does not return any error message
        /// </summary>
        /// <param name="entity"></param>
        public void AssertUpdate(BudgetClass entity)
        {
            var errors = TryUpdate(entity);

            errors.Should().BeEmpty();
        }

        /// <summary>
        /// Gets the entity using the KeyDataExpression from the Data class
        /// </summary>
        /// <param name="data">Data for the entity to be searched</param>
        /// <returns>Entity</returns>
        public BudgetClass GetEntity(BudgetClassData data)
        {
            using (var dbContext = DbSetup.GetDbContext())
            {
                var manager = new BudgetClassManager(dbContext);

                return manager.SingleOrDefault(data.KeyExpression);
            }
        }

        /// <summary>
        /// Executes the TryDelete method and returns the validation results or saves the changes if ok.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Validation Results</returns>
        public IEnumerable<ValidationResult> TryDelete(BudgetClass entity)
        {
            using (var dbContext = DbSetup.GetDbContext())
            {
                var manager = new BudgetClassManager(dbContext);

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
        public IEnumerable<ValidationResult> TryInsert(BudgetClass entity)
        {
            using (var dbContext = DbSetup.GetDbContext())
            {
                var manager = new BudgetClassManager(dbContext);

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
        public IEnumerable<ValidationResult> TryUpdate(BudgetClass entity)
        {
            using (var dbContext = DbSetup.GetDbContext())
            {
                var manager = new BudgetClassManager(dbContext);

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
