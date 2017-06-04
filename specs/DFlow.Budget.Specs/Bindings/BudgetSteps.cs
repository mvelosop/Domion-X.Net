﻿using Autofac;
using DFlow.Budget.Lib.Services;
using DFlow.Budget.Lib.Tests.Helpers;
using FluentAssertions;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace DFlow.Budget.Specs.Bindings
{
    [Binding]
    public sealed class BudgetSteps
    {
        // For additional details on SpecFlow step definitions see http://go.specflow.org/doc-stepdef

        public BudgetClassManager BudgetClassManager => Resolve<BudgetClassManager>();

        public BudgetClassManagerHelper BudgetClassManagerHelper => Resolve<BudgetClassManagerHelper>();

        [Given(@"the following budget classes do not exist:")]
        public void GivenTheFollowingBudgetClassesDoNotExist(Table table)
        {
            BudgetClassData[] dataSet = table.CreateSet<BudgetClassData>().ToArray();

            BudgetClassManagerHelper.EnsureEntitiesDoNotExist(dataSet);
        }

        [Then(@"I can find the following budget classes starting with ""(.*)"":")]
        public void ThenICanFindTheFollowingBudgetClassesStartingWith(string queryText, Table table)
        {
            var dataSet = BudgetClassManager
                .Query(bc => bc.Name.StartsWith(queryText))
                .ToList()
                .Select(bc => new BudgetClassData(bc));

            table.CompareToSet(dataSet);
        }

        [When(@"I add the following budget classes:")]
        public void WhenIAddTheFollowingBudgetClasses(Table table)
        {
            BudgetClassData[] dataSet = table.CreateSet<BudgetClassData>().ToArray();

            foreach (var data in dataSet)
            {
                var entity = data.CreateEntity();

                var errors = BudgetClassManager.TryInsert(entity);

                errors.Should().BeEmpty();
            }

            BudgetClassManager.SaveChanges();
        }

        private T Resolve<T>()
        {
            return ScenarioContext.Current.Get<ILifetimeScope>(BudgetHooks.containerTag).Resolve<T>();
        }
    }
}
