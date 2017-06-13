using Autofac;
using DFlow.Budget.Core.Model;
using DFlow.Budget.Lib.Services;
using Domion.Lib.Extensions;
using FluentAssertions;
using FluentAssertions.Equivalency;
using System;
using System.Linq;

namespace DFlow.Budget.Lib.Tests.Helpers
{
    /// <summary>
    ///     <para>
    ///         Test helper class for BudgetLineManager.
    ///     </para>
    ///
    ///     <para>
    ///         Has to be used within an Autofac ILifetimeScope. Manages entity class "BudgetLine" using data class "BudgetLineData" as input
    ///     </para>
    /// </summary>
    public class BudgetLineManagerHelper
    {
        private readonly Func<EquivalencyAssertionOptions<BudgetLineData>, EquivalencyAssertionOptions<BudgetLineData>> _dataEquivalenceOptions =
            options => options
                .Excluding(si => si.SelectedMemberPath.EndsWith("_Id"));

        private readonly Lazy<BudgetLineDataHelper> _lazyBudgetLineDataHelper;
        private readonly Lazy<BudgetLineManager> _lazyBudgetLineManager;
        private readonly ILifetimeScope _scope;

        /// <summary>
        /// Creates the test helper for BudgetLineManager
        /// </summary>
        /// <param name="lazyBudgetLineManager"></param>
        public BudgetLineManagerHelper(
            ILifetimeScope scope,
            Lazy<BudgetLineDataHelper> lazyBudgetLineDataHelper,
            Lazy<BudgetLineManager> lazyBudgetLineManager)
        {
            _scope = scope;

            _lazyBudgetLineDataHelper = lazyBudgetLineDataHelper;
            _lazyBudgetLineManager = lazyBudgetLineManager;
        }

        private BudgetLineDataHelper BudgetLineDataHelper => _lazyBudgetLineDataHelper.Value;

        private BudgetLineManager BudgetLineManager => _lazyBudgetLineManager.Value;

        /// <summary>
        /// Asserts that entities with the supplied key data values do not exist
        /// </summary>
        /// <param name="dataSet"></param>
        public void AssertEntitiesDoNotExist(params BudgetLineData[] dataSet)
        {
            using (var scope = GetLocalScope())
            {
                var manager = scope.Resolve<BudgetLineManager>();

                foreach (var data in dataSet)
                {
                    var entity = manager.SingleOrDefault(e => e.Name == data.Name);

                    entity.Should().BeNull(@"because BudgetLine ""{0}"" MUST NOT EXIST!", data.Name);
                }
            }
        }

        /// <summary>
        /// Asserts that entities equivalent to the supplied input data classes exist
        /// </summary>
        /// <param name="dataSet"></param>
        public void AssertEntitiesExist(params BudgetLineData[] dataSet)
        {
            using (var scope = GetLocalScope())
            {
                var manager = scope.Resolve<BudgetLineManager>();

                foreach (var data in dataSet)
                {
                    BudgetLine entity = manager.Include(e => e.BudgetClass).SingleOrDefault(e => e.Name == data.Name);

                    entity.Should().NotBeNull(@"because BudgetLine ""{0}"" MUST EXIST!", data.Name);

                    var entityData = new BudgetLineData(entity);

                    entityData.ShouldBeEquivalentTo(data, options => _dataEquivalenceOptions(options));
                }
            }
        }

        /// <summary>
        /// Ensures that the entities do not exist in the database or are succesfully removed
        /// </summary>
        /// <param name="dataSet">Data for the entities to be searched and removed</param>
        public void EnsureEntitiesDoNotExist(params BudgetLineData[] dataSet)
        {
            foreach (var data in dataSet)
            {
                var entity = BudgetLineManager.SingleOrDefault(e => e.Name == data.Name);

                if (entity != null)
                {
                    var errors = BudgetLineManager.TryDelete(entity);

                    errors.Should().BeEmpty(@"because BudgetClass ""{0}"" has to be removed!", data.Name);
                }
            }

            BudgetLineManager.SaveChanges();

            AssertEntitiesDoNotExist(dataSet);
        }

        /// <summary>
        /// Ensures that the entities exist in the database or are succesfully added
        /// </summary>
        /// <param name="dataSet"></param>
        public void EnsureEntitiesExist(params BudgetLineData[] dataSet)
        {
            foreach (var data in dataSet)
            {
                var entity = BudgetLineManager.SingleOrDefault(e => e.Name == data.Name);

                if (entity == null)
                {
                    entity = data.CreateEntity(BudgetLineDataHelper);

                    var errors = BudgetLineManager.TryInsert(entity);

                    errors.Should().BeEmpty(@"because BudgetLine ""{0}"" has to be added!", data.Name);
                }
            }

            BudgetLineManager.SaveChanges();

            AssertEntitiesExist(dataSet);
        }

        private ILifetimeScope GetLocalScope()
        {
            return _scope.BeginLifetimeScope();
        }
    }
}
