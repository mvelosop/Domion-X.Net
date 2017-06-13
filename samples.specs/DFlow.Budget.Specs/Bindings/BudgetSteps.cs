using Autofac;
using DFlow.Budget.Lib.Services;
using DFlow.Budget.Lib.Tests.Helpers;
using DFlow.Tennants.Lib.Services;
using DFlow.Tennants.Lib.Tests.Helpers;
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

        private ScenarioContext _scenarioContext;

        public BudgetSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        public BudgetClassManager BudgetClassManager => Resolve<BudgetClassManager>();

        public BudgetClassManagerHelper BudgetClassManagerHelper => Resolve<BudgetClassManagerHelper>();

        public TennantManager TennantManager => Resolve<TennantManager>();
        public TennantManagerHelper TennantManagerHelper => Resolve<TennantManagerHelper>();

        [Given(@"the current user is working as tennant ""(.*)""")]
        public void GivenTheCurrentUserIsWorkingAsTennant(string owner)
        {
            var data = new TennantData(owner);

            TennantManagerHelper.EnsureEntitiesExist(data);

            var currentTennant = TennantManager.AssertGetByKeyData(owner);

            _scenarioContext.Add("CurrentTennant", currentTennant);
        }


        [Given(@"there are no registered budget classes")]
        public void GivenThereAreNoRegisteredBudgetClasses()
        {
            var entities = BudgetClassManager.Query().ToList();

            foreach (var entity in entities)
            {
                var errors = BudgetClassManager.TryDelete(entity);

                errors.Should().BeEmpty();
            }

            BudgetClassManager.SaveChanges();
        }

        [Then(@"I can find the following budget classes:")]
        public void ThenICanFindTheFollowingBudgetClasses(Table table)
        {
            var dataSet = BudgetClassManager
                .Query()
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
            return _scenarioContext.Get<ILifetimeScope>(BudgetHooks.containerTag).Resolve<T>();
        }
    }
}
