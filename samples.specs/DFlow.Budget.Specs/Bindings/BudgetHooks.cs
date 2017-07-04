using Autofac;
using DFlow.Budget.Setup;
using DFlow.Tenants.Core.Model;
using DFlow.Tenants.Setup;
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
        private static TenantsDbSetupHelper _TenantsDbSetupHelper;

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
            _TenantsDbSetupHelper = new TenantsDbSetupHelper(connectionString);
            _budgetDbSetupHelper = new BudgetDbSetupHelper(connectionString);

            _TenantsDbSetupHelper.SetupDatabase();
            _budgetDbSetupHelper.SetupDatabase();
        }

        private IContainer SetupContainer()
        {
            var builder = new ContainerBuilder();

            var budgetAutofacHelper = new BudgetContainerSetup(_budgetDbSetupHelper);
            var TenantsAutofacHelper = new TenantsAutofacSetupHelper(_TenantsDbSetupHelper);

            TenantsAutofacHelper.SetupContainer(builder);
            budgetAutofacHelper.RegisterTypes(builder);

            builder.Register<Tenant>((c) => _scenarioContext.Get<Tenant>("CurrentTenant"));

            IContainer container = builder.Build();

            return container;
        }
    }
}
