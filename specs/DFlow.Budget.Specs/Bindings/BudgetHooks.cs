using Autofac;
using DFlow.Budget.Setup;
using TechTalk.SpecFlow;

namespace DFlow.Budget.Specs.Bindings
{
    [Binding]
    public sealed class BudgetHooks
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        public static string containerTag = "AutofacContainer";

        private static BudgetDbSetupHelper _dbHelper;
        private static string _defaultConnectionString = "Data Source=localhost;Initial Catalog=DFlow.Budget.Lib.Specs;Integrated Security=SSPI;MultipleActiveResultSets=true";
        private static IContainer _testContainer;

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            _dbHelper = new BudgetDbSetupHelper(_defaultConnectionString);

            _dbHelper.SetupDatabase();

            var autofacHelper = new BudgetAutofacSetupHelper(_dbHelper);

            var builder = new ContainerBuilder();

            autofacHelper.SetupContainer(builder);

            _testContainer = builder.Build();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            //TODO: implement logic that has to run after executing each scenario
        }

        [AfterStep]
        public void AfterStep()
        {
            var scope = ScenarioContext.Current.Get<ILifetimeScope>(containerTag);

            scope.Dispose();

            ScenarioContext.Current.Remove(containerTag);
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            //TODO: implement logic that has to run before executing each scenario
        }

        [BeforeStep]
        public void BeforeStep()
        {
            var scope = _testContainer.BeginLifetimeScope();

            ScenarioContext.Current.Add(containerTag, scope);
        }
    }
}
