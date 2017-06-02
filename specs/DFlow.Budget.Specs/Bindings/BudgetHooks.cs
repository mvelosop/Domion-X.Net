using Autofac;
using BoDi;
using DFlow.Budget.Lib.Data;
using DFlow.Budget.Lib.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TechTalk.SpecFlow;

namespace DFlow.Budget.Specs.Bindings
{
    [Binding]
    public sealed class BudgetHooks
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        public static string containerTag = "AutofacContainer";

        private static string _defaultConnectionString = "Data Source=localhost;Initial Catalog=DFlow.Budget.Lib.Specs;Integrated Security=SSPI;MultipleActiveResultSets=true";
        private static IContainer _testContainer;
        private static BudgetDbSetupHelper _dbHelper;

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            _dbHelper = new BudgetDbSetupHelper(_defaultConnectionString);

            _dbHelper.SetupDatabase();

            SetupDependencyResolver();
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

        private static Assembly[] GetLoadedAssemblies()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;

            Assembly[] appAssemblies = currentDomain.GetAssemblies().Where(a => a.GetName().Name.StartsWith("DFlow")).ToArray();

            return appAssemblies;
        }

        private static void RegisterTypes(Assembly[] appAssemblies)
        {
            var builder = new ContainerBuilder();

            builder.Register<BudgetDbContext>((c) => _dbHelper.GetDbContext())
                .InstancePerLifetimeScope();

            foreach (var asm in appAssemblies)
            {
                builder.RegisterAssemblyTypes(asm)
                    .Where(t => t.Name.EndsWith("Manager"))
                    .InstancePerLifetimeScope()
                    .AsSelf()
                    .AsImplementedInterfaces();

                //builder.RegisterAssemblyTypes(asm)
                //    .Where(t => t.Name.EndsWith("DbContext"))
                //    .InstancePerLifetimeScope();

                builder.RegisterAssemblyTypes(asm)
                    .Where(t => t.Name.EndsWith("ManagerHelper"))
                    .InstancePerLifetimeScope();
            }

            _testContainer = builder.Build();
        }

        private static void SetupDependencyResolver()
        {
            Assembly[] appAssemblies = GetLoadedAssemblies();

            RegisterTypes(appAssemblies);
        }
    }
}
