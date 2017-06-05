using DFlow.Budget.Lib.Services;
using System;

namespace DFlow.Budget.Lib.Tests.Helpers
{
    public class BudgetLineDataHelper
    {
        private Lazy<BudgetClassManager> _lazyBudgetClassManager;

        public BudgetLineDataHelper(
            Lazy<BudgetClassManager> lazyBudgetClassManager)
        {
            _lazyBudgetClassManager = lazyBudgetClassManager;
        }

        private BudgetClassManager BudgetClassManager { get { return _lazyBudgetClassManager.Value; } }

        public void SetReferences(BudgetLineData data)
        {
            data.BudgetClass_Id = BudgetClassManager.AssertGetByKeyData(data.ClassName).Id;
        }
    }
}
