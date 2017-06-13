using Autofac;
using DFlow.Budget.Setup;
using DFlow.Tennants.Core.Model;
using DFlow.Tennants.Setup;
using TechTalk.SpecFlow;

namespace DFlow.Budget.Specs.Bindings
{
    [Binding]
    public sealed class BudgetHooks
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        public const string containerTag = "AutofacContainer";

        private static BudgetDbSetupHelper _budgetDbSetupHelper;
        private static string _connectionString = "Data Source=localhost;Initial Catalog=DFlow.Budget.Lib.Specs;Integrated Security=SSPI;MultipleActiveResultSets=true";
        private static TennantsDbSetupHelper _tennantsDbSetupHelper;

        private IContainer _container;
        private ScenarioContext _scenarioContext;

        public BudgetHooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            SetupDatabase(_connectionString);
        }

        [AfterScenario]
        public void AfterScenario()
        {
            _container.Dispose();
        }

        [AfterStep]
        public void AfterStep()
        {
            ILifetimeScope scope;

            if (_scenarioContext.TryGetValue<ILifetimeScope>(containerTag, out scope))
            {
                if (scope != null)
                {
                    scope.Dispose();
                }

                _scenarioContext.Remove(containerTag);
            }
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _container = SetupContainer();
        }

        [BeforeStep]
        public void BeforeStep()
        {
            var scope = _container.BeginLifetimeScope();

            _scenarioContext.Add(containerTag, scope);
        }

        private static void SetupDatabase(string connectionString)
        {
            _tennantsDbSetupHelper = new TennantsDbSetupHelper(connectionString);
            _budgetDbSetupHelper = new BudgetDbSetupHelper(connectionString);

            _tennantsDbSetupHelper.SetupDatabase();
            _budgetDbSetupHelper.SetupDatabase();
        }

        private IContainer SetupContainer()
        {
            var builder = new ContainerBuilder();

            var budgetAutofacHelper = new BudgetAutofacSetupHelper(_budgetDbSetupHelper);
            var tennantsAutofacHelper = new TennantsAutofacSetupHelper(_tennantsDbSetupHelper);

            tennantsAutofacHelper.SetupContainer(builder);
            budgetAutofacHelper.SetupContainer(builder);

            builder.Register<Tennant>((c) => _scenarioContext.Get<Tennant>("CurrentTennant"));

            IContainer container = builder.Build();

            return container;
        }
    }
}
