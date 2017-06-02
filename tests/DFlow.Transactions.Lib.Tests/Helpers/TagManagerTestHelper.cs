using DFlow.Transactions.Core.Model;
using DFlow.Transactions.Lib.Services;
using FluentAssertions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DFlow.Transactions.Lib.Tests.Helpers
{
    /// <summary>
    /// Test helper class for TagManager
    ///
    /// Takes a TransactionsDbSetup to execute CRUD methods and dispose properly the DbContext.
    /// Manages entity class Tag using data class TagData as input
    /// </summary>
    public class TagManagerTestHelper
    {
        /// <summary>
        /// Creates the test helper for TagManager
        /// </summary>
        /// <param name="dbSetup"></param>
        public TagManagerTestHelper(TransactionsDbSetupHelper dbSetup)
        {
            DbSetup = dbSetup;
        }

        public TransactionsDbSetupHelper DbSetup { get; set; }

        /// <summary>
        /// Asserts that TryDelete method does not return any error message
        /// </summary>
        /// <param name="entity"></param>
        public void AssertDelete(Tag entity)
        {
            var errors = TryDelete(entity);

            errors.Should().BeEmpty();
        }

        /// <summary>
        /// Asserts that the entities do not exist in the database or are succesfully removed
        /// </summary>
        /// <param name="dataArray">Data for the entities to be searched and removed</param>
        public void AssertEntitiesDoNotExist(params TagData[] dataArray)
        {
            using (var dbContext = DbSetup.GetDbContext())
            {
                var manager = new TagManager(dbContext);

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
                var manager = new TagManager(dbContext);

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
        public void AssertInsert(Tag entity)
        {
            var errors = TryInsert(entity);

            errors.Should().BeEmpty();
        }

        /// <summary>
        /// Asserts that TryUpdate method does not return any error message
        /// </summary>
        /// <param name="entity"></param>
        public void AssertUpdate(Tag entity)
        {
            var errors = TryUpdate(entity);

            errors.Should().BeEmpty();
        }

        /// <summary>
        /// Gets the entity using the KeyDataExpression from the Data class
        /// </summary>
        /// <param name="data">Data for the entity to be searched</param>
        /// <returns>Entity</returns>
        public Tag GetEntity(TagData data)
        {
            using (var dbContext = DbSetup.GetDbContext())
            {
                var manager = new TagManager(dbContext);

                return manager.SingleOrDefault(data.KeyExpression);
            }
        }

        /// <summary>
        /// Executes the TryDelete method and returns the validation results or saves the changes if ok.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Validation Results</returns>
        public IEnumerable<ValidationResult> TryDelete(Tag entity)
        {
            using (var dbContext = DbSetup.GetDbContext())
            {
                var manager = new TagManager(dbContext);

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
        public IEnumerable<ValidationResult> TryInsert(Tag entity)
        {
            using (var dbContext = DbSetup.GetDbContext())
            {
                var manager = new TagManager(dbContext);

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
        public IEnumerable<ValidationResult> TryUpdate(Tag entity)
        {
            using (var dbContext = DbSetup.GetDbContext())
            {
                var manager = new TagManager(dbContext);

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
