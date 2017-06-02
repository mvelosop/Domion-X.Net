using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace DFlow.Budget.Specs.Bindings
{
    [Binding]
    public sealed class BudgetSteps
    {
        // For additional details on SpecFlow step definitions see http://go.specflow.org/doc-stepdef

        [Given(@"the following budget classes do not exist:")]
        public void GivenTheFollowingBudgetClassesDoNotExist(Table table)
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I add the following budget classes:")]
        public void WhenIAddTheFollowingBudgetClasses(Table table)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"I can find the following budget classes starting with ""(.*)"":")]
        public void ThenICanFindTheFollowingBudgetClassesStartingWith(string queryText, Table table)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
