using DFlow.Budget.Lib.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace DFlow.Budget.Specs.Bindings
{
    [Binding]
    public sealed class BudgetHooks
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        private static string _defaultConnectionString = "Data Source=localhost;Initial Catalog=DFlow.Budget.Lib.Specs;Integrated Security=SSPI;MultipleActiveResultSets=true";

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            var dbHelper = new BudgetDbSetupHelper(_defaultConnectionString);

            dbHelper.SetupDatabase();
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            //TODO: implement logic that has to run before executing each scenario
        }

        [AfterScenario]
        public void AfterScenario()
        {
            //TODO: implement logic that has to run after executing each scenario
        }
    }
}
