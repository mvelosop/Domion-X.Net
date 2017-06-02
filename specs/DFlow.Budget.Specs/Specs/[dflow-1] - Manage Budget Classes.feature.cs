﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.1.0.0
//      SpecFlow Generator Version:2.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace DFlow.Budget.Specs.Specs
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.1.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class Dflow_1_Feature_ManageBudgetClassesFeature : Xunit.IClassFixture<Dflow_1_Feature_ManageBudgetClassesFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "[dflow-1] - Manage Budget Classes.feature"
#line hidden
        
        public Dflow_1_Feature_ManageBudgetClassesFeature()
        {
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "(dflow-1) - Feature - Manage Budget Classes", "\tIn order to manage my personal budget\r\n\tAs the one responsible to do it\r\n\tI want" +
                    " to manage a list of general budget classes", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void TestInitialize()
        {
        }
        
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void SetFixture(Dflow_1_Feature_ManageBudgetClassesFeature.FixtureData fixtureData)
        {
        }
        
        void System.IDisposable.Dispose()
        {
            this.ScenarioTearDown();
        }
        
        [Xunit.FactAttribute(DisplayName="(dflow-1) - Scenario - Add new budget classes")]
        [Xunit.TraitAttribute("FeatureTitle", "(dflow-1) - Feature - Manage Budget Classes")]
        [Xunit.TraitAttribute("Description", "(dflow-1) - Scenario - Add new budget classes")]
        public virtual void Dflow_1_Scenario_AddNewBudgetClasses()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("(dflow-1) - Scenario - Add new budget classes", ((string[])(null)));
#line 6
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Type"});
            table1.AddRow(new string[] {
                        "New - Income",
                        "Income"});
            table1.AddRow(new string[] {
                        "New - Housing",
                        "Expense"});
            table1.AddRow(new string[] {
                        "New - Transportation",
                        "Expense"});
            table1.AddRow(new string[] {
                        "New - Savings",
                        "Savings"});
            table1.AddRow(new string[] {
                        "New - Investment",
                        "Investment"});
            table1.AddRow(new string[] {
                        "New - Taxes",
                        "Taxes"});
            table1.AddRow(new string[] {
                        "New - Loans",
                        "Loans"});
#line 8
 testRunner.Given("the following budget classes do not exist:", ((string)(null)), table1, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Type"});
            table2.AddRow(new string[] {
                        "New - Income",
                        "Income"});
            table2.AddRow(new string[] {
                        "New - Housing",
                        "Expense"});
            table2.AddRow(new string[] {
                        "New - Transportation",
                        "Expense"});
            table2.AddRow(new string[] {
                        "New - Savings",
                        "Savings"});
            table2.AddRow(new string[] {
                        "New - Investment",
                        "Investment"});
            table2.AddRow(new string[] {
                        "New - Taxes",
                        "Taxes"});
            table2.AddRow(new string[] {
                        "New - Loans",
                        "Loans"});
#line 18
 testRunner.When("I add the following budget classes:", ((string)(null)), table2, "When ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Type"});
            table3.AddRow(new string[] {
                        "New - Income",
                        "Income"});
            table3.AddRow(new string[] {
                        "New - Housing",
                        "Expense"});
            table3.AddRow(new string[] {
                        "New - Transportation",
                        "Expense"});
            table3.AddRow(new string[] {
                        "New - Savings",
                        "Savings"});
            table3.AddRow(new string[] {
                        "New - Investment",
                        "Investment"});
            table3.AddRow(new string[] {
                        "New - Taxes",
                        "Taxes"});
            table3.AddRow(new string[] {
                        "New - Loans",
                        "Loans"});
#line 28
 testRunner.Then("I can find the following budget classes starting with \"New - \":", ((string)(null)), table3, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.1.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                Dflow_1_Feature_ManageBudgetClassesFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                Dflow_1_Feature_ManageBudgetClassesFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
