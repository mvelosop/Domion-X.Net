using Autofac;
using DFlow.Budget.Core.Model;
using DFlow.Budget.Lib.Services;
using Domion.Lib.Extensions;
using FluentAssertions;
using FluentAssertions.Equivalency;
using System;

namespace DFlow.Budget.Lib.Tests.Helpers
{
    /// <summary>
    ///     <para>
    ///         Test helper class for BudgetClassManager.
    ///     </para>
    ///
    ///     <para>
    ///         Has to be used within an Autofac ILifetimeScope. Manages entity class "BudgetClass" using data class "BudgetClassData" as input
    ///     </para>
    /// </summary>
    public class BudgetClassManagerHelper
    {
        private readonly Func<EquivalencyAssertionOptions<BudgetClassData>, EquivalencyAssertionOptions<BudgetClassData>> _dataEquivalenceOptions =
            options => options
                .Excluding(si => si.SelectedMemberPath.EndsWith("_Id"));

        private readonly Lazy<BudgetClassManager> _lazyBudgetClassManager;

        private readonly ILifetimeScope _scope;

        /// <summary>
        /// Creates the test helper for BudgetClassManager
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="lazyBudgetClassManager"></param>
        public BudgetClassManagerHelper(
            ILifetimeScope scope,
            Lazy<BudgetClassManager> lazyBudgetClassManager)
        {
            _scope = scope;

            _lazyBudgetClassManager = lazyBudgetClassManager;
        }

        private BudgetClassManager BudgetClassManager => _lazyBudgetClassManager.Value;

        /// <summary>
        /// Asserts that entities with the supplied key data values do not exist
        /// </summary>
        /// <param name="dataSet"></param>
        public void AssertEntitiesDoNotExist(params BudgetClassData[] dataSet)
        {
            using (ILifetimeScope scope = GetLocalScope(_scope))
            {
                var manager = scope.Resolve<BudgetClassManager>();

                foreach (BudgetClassData data in dataSet)
                {
                    BudgetClass entity = manager.SingleOrDefault(e => e.Name == data.Name);

                    entity.Should().BeNull(@"because BudgetClass ""{0}"" MUST NOT EXIST!", data.Name);
                }
            }
        }

        /// <summary>
        /// Asserts that entities equivalent to the supplied input data classes exist
        /// </summary>
        /// <param name="dataSet"></param>
        public void AssertEntitiesExist(params BudgetClassData[] dataSet)
        {
            using (ILifetimeScope scope = GetLocalScope(_scope))
            {
                var manager = scope.Resolve<BudgetClassManager>();

                foreach (BudgetClassData data in dataSet)
                {
                    BudgetClass entity = manager.SingleOrDefault(e => e.Name == data.Name);

                    entity.Should().NotBeNull(@"because BudgetClass ""{0}"" MUST EXIST!", data.Name);

                    var entityData = new BudgetClassData(entity);

                    entityData.ShouldBeEquivalentTo(data, options => _dataEquivalenceOptions(options));
                }
            }
        }

        /// <summary>
        /// Ensures that the entities do not exist in the database or are succesfully removed
        /// </summary>
        /// <param name="dataSet">Data for the entities to be searched and removed</param>
        public void EnsureEntitiesDoNotExist(params BudgetClassData[] dataSet)
        {
            foreach (BudgetClassData data in dataSet)
            {
                BudgetClass entity = BudgetClassManager.SingleOrDefault(e => e.Name == data.Name);

                if (entity == null) continue;

                var errors = BudgetClassManager.TryDelete(entity);

                errors.Should().BeEmpty(@"because BudgetClass ""{0}"" has to be removed!", data.Name);
            }

            BudgetClassManager.SaveChanges();

            AssertEntitiesDoNotExist(dataSet);
        }

        /// <summary>
        /// Ensures that the entities exist in the database or are succesfully added
        /// </summary>
        /// <param name="dataSet"></param>
        public void EnsureEntitiesExist(params BudgetClassData[] dataSet)
        {
            foreach (BudgetClassData data in dataSet)
            {
                BudgetClass entity = BudgetClassManager.SingleOrDefault(e => e.Name == data.Name);

                if (entity != null) continue;

                entity = data.CreateEntity();

                var errors = BudgetClassManager.TryInsert(entity);

                errors.Should().BeEmpty(@"because BudgetClass ""{0}"" has to be added!", data.Name);
            }

            BudgetClassManager.SaveChanges();

            AssertEntitiesExist(dataSet);
        }

        private ILifetimeScope GetLocalScope(ILifetimeScope scope = null)
        {
            ILifetimeScope localScope = scope ?? _scope;

            return localScope.BeginLifetimeScope();
        }
    }
}
