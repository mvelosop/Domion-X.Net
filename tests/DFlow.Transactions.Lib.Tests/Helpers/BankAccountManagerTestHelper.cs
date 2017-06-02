using DFlow.Transactions.Core.Model;
using DFlow.Transactions.Lib.Services;
using FluentAssertions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DFlow.Transactions.Lib.Tests.Helpers
{
    /// <summary>
    ///     <para>
    ///         Test helper class for BankAccountManager.
    ///     </para>
    ///     <para>
    ///         Takes a TransactionsDbSetupHelper to execute CRUD methods, disposing the DbContext.
    ///         Manages entity class BankAccount using data class BankAccountData as input.
    ///     </para>
    /// </summary>
    public class BankAccountManagerTestHelper
    {
        /// <summary>
        /// Creates the test helper for BankAccountManager
        /// </summary>
        /// <param name="dbSetup"></param>
        public BankAccountManagerTestHelper(TransactionsDbSetupHelper dbSetup)
        {
            DbSetup = dbSetup;
        }

        public TransactionsDbSetupHelper DbSetup { get; set; }

        /// <summary>
        /// Asserts that TryDelete method did not return any error message
        /// </summary>
        /// <param name="entity"></param>
        public void AssertDelete(BankAccount entity)
        {
            var errors = TryDelete(entity);

            errors.Should().BeEmpty();
        }

        /// <summary>
        /// Asserts that the entities do not exist in the database or are succesfully removed
        /// </summary>
        /// <param name="dataArray">Data for the entities to be searched and removed</param>
        public void AssertEntitiesDoNotExist(params BankAccountData[] dataArray)
        {
            using (var dbContext = DbSetup.GetDbContext())
            {
                var manager = new BankAccountManager(dbContext);

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
                var manager = new BankAccountManager(dbContext);

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
        public void AssertInsert(BankAccount entity)
        {
            var errors = TryInsert(entity);

            errors.Should().BeEmpty();
        }

        /// <summary>
        /// Asserts that TryUpdate method does not return any error message
        /// </summary>
        /// <param name="entity"></param>
        public void AssertUpdate(BankAccount entity)
        {
            var errors = TryUpdate(entity);

            errors.Should().BeEmpty();
        }

        /// <summary>
        /// Gets the entity using the KeyDataExpression from the Data class
        /// </summary>
        /// <param name="data">Data for the entity to be searched</param>
        /// <returns>Entity</returns>
        public BankAccount GetEntity(BankAccountData data)
        {
            using (var dbContext = DbSetup.GetDbContext())
            {
                var manager = new BankAccountManager(dbContext);

                return manager.SingleOrDefault(data.KeyExpression);
            }
        }

        /// <summary>
        /// Executes the TryDelete method and returns the validation results or saves the changes if ok.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Validation Results</returns>
        public IEnumerable<ValidationResult> TryDelete(BankAccount entity)
        {
            using (var dbContext = DbSetup.GetDbContext())
            {
                var manager = new BankAccountManager(dbContext);

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
        public IEnumerable<ValidationResult> TryInsert(BankAccount entity)
        {
            using (var dbContext = DbSetup.GetDbContext())
            {
                var manager = new BankAccountManager(dbContext);

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
        public IEnumerable<ValidationResult> TryUpdate(BankAccount entity)
        {
            using (var dbContext = DbSetup.GetDbContext())
            {
                var manager = new BankAccountManager(dbContext);

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
