using Demo.Budget.Core.Model;

namespace Demo.Budget.Lib.Tests.Helpers
{
    public class BudgetClassData
    {
        public BudgetClassData()
        {
        }

        public BudgetClassData(BudgetClass entity)
        {
            Name = entity.Name;
        }

        public BudgetClassData(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public BudgetClass CreateEntity()
        {
            return new BudgetClass
            {
                Name = Name
            };
        }
    }
}
