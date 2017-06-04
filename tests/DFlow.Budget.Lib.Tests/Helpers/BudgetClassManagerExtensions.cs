using DFlow.Budget.Core.Model;
using DFlow.Budget.Lib.Services;
using FluentAssertions;

namespace DFlow.Budget.Lib.Tests.Helpers
{
    public static class BudgetClassManagerExtensions
    {
        /// <summary>
        /// Asserts that TryDelete method does not return any error message
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="entity"></param>
        public static void AssertCanDelete(this BudgetClassManager manager, BudgetClass entity)
        {
            var errors = manager.TryDelete(entity);

            errors.Should().BeEmpty();
        }

        /// <summary>
        /// Asserts that TryInsert method does not return any error message
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="entity"></param>
        public static void AssertCanInsert(this BudgetClassManager manager, BudgetClass entity)
        {
            var errors = manager.TryInsert(entity);

            errors.Should().BeEmpty();
        }

        /// <summary>
        /// Asserts that TryUpdate method does not return any error message
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="entity"></param>
        public static void AssertCanUpdate(this BudgetClassManager manager, BudgetClass entity)
        {
            var errors = manager.TryUpdate(entity);

            errors.Should().BeEmpty();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static BudgetClassData GetDataByKeyValue(this BudgetClassManager manager, string name)
        {
            BudgetClass entity = GetEntityByKeyValue(manager, name);

            if (entity == null)
            {
                return null;
            }
            else
            {
                return new BudgetClassData(entity);
            }
        }

        /// <summary>
        /// Gets the entity by KeyValue
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="name">Key value to search for the entity</param>
        /// <returns></returns>
        public static BudgetClass GetEntityByKeyValue(this BudgetClassManager manager, string name)
        {
            BudgetClass entity = manager.SingleOrDefault(c => c.Name == name);

            return entity;
        }
    }
}
