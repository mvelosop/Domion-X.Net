using DFlow.Budget.Core.Model;
using DFlow.Budget.Lib.Services;
using FluentAssertions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System;
using Autofac;
using FluentAssertions.Collections;
using FluentAssertions.Equivalency;

namespace DFlow.Budget.Lib.Tests.Helpers
{
    /// <summary>
    /// Test helper class for BudgetClassManager
    ///
    /// Takes a BudgetDbSetup to execute CRUD methods and dispose properly the DbContext.
    /// Manages entity class BudgetClass using data class BudgetClassData as input
    /// </summary>
    public class BudgetClassManagerHelper
    {
        private Func<EquivalencyAssertionOptions<BudgetClassData>, EquivalencyAssertionOptions<BudgetClassData>> _dataEquivalenceOptions =
            options => options
                .Excluding(si => si.SelectedMemberPath.EndsWith("_Id"));

        private Lazy<BudgetClassManager> _lazyBudgetClassManager;

        private ILifetimeScope _scope;

        /// <summary>
        /// Creates the test helper for BudgetManager
        /// </summary>
        /// <param name="lazyBudgetClassManager"></param>
        public BudgetClassManagerHelper(
            ILifetimeScope scope,
            Lazy<BudgetClassManager> lazyBudgetClassManager)
        {
            _scope = scope;

            _lazyBudgetClassManager = lazyBudgetClassManager;
        }

        private BudgetClassManager BudgetClassManager { get { return _lazyBudgetClassManager.Value; } }

        public void AssertEntitiesDoNotExist(params BudgetClassData[] dataSet)
        {
            using (var scope = GetLocalScope())
            {
                var manager = scope.Resolve<BudgetClassManager>();

                foreach (var data in dataSet)
                {
                    var entity = manager.GetEntityByKeyValue(data.Name);

                    entity.Should().BeNull(@"because BudgetClass ""{0}"" MUST NOT EXIST!", data.Name);
                }
            }
        }

        public void AssertEntitiesExist(params BudgetClassData[] dataSet)
        {
            using (var scope = GetLocalScope())
            {
                var manager = scope.Resolve<BudgetClassManager>();

                foreach (var data in dataSet)
                {
                    var savedData = manager.GetDataByKeyValue(data.Name);

                    savedData.Should().NotBeNull(@"because BudgetClass ""{0}"" MUST EXIST!", data.Name);

                    savedData.ShouldBeEquivalentTo(data, options => _dataEquivalenceOptions(options));
                }
            }
        }

        /// <summary>
        /// Asserts that the entities do not exist in the database or are succesfully removed
        /// </summary>
        /// <param name="dataSet">Data for the entities to be searched and removed</param>
        public void EnsureEntitiesDoNotExist(params BudgetClassData[] dataSet)
        {
            foreach (var data in dataSet)
            {
                var entity = BudgetClassManager.GetEntityByKeyValue(data.Name);

                if (entity != null)
                {
                    var errors = BudgetClassManager.TryDelete(entity);

                    errors.Should().BeEmpty(@"because BudgetClass ""{0}"" has to be removed!", data.Name);
                }
            }

            BudgetClassManager.SaveChanges();

            AssertEntitiesDoNotExist(dataSet);
        }

        public void EnsureEntitiesExist(params BudgetClassData[] dataSet)
        {
            foreach (var data in dataSet)
            {
                var entity = BudgetClassManager.GetEntityByKeyValue(data.Name);

                if (entity == null)
                {
                    entity = data.CreateEntity();

                    var errors = BudgetClassManager.TryInsert(entity);

                    errors.Should().BeEmpty(@"because BudgetClass ""{0}"" has to be added!", data.Name);
                }
            }

            BudgetClassManager.SaveChanges();

            AssertEntitiesExist(dataSet);
        }

        private ILifetimeScope GetLocalScope()
        {
            return _scope.BeginLifetimeScope();
        }
    }
}
