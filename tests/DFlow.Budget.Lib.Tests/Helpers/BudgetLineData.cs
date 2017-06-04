using DFlow.Budget.Core.Model;
using System;
using System.Linq.Expressions;

namespace DFlow.Budget.Lib.Tests.Helpers
{
    public class BudgetLineData
    {
        public BudgetLineData()
        {
        }

        public BudgetLineData(string name, string className)
        {
            Name = name;

            ClassName = className;
        }

        public BudgetLineData(BudgetLine entity)
        {
            Name = entity.Name;

            ClassName = entity.BudgetClass.Name;

            BudgetClass_Id = entity.BudgetClass_Id;
        }

        public int BudgetClass_Id { get; set; }

        public string ClassName { get; set; }

        public string Name { get; set; }

        public BudgetLine CreateEntity()
        {
            return new BudgetLine
            {
                Name = Name,

                BudgetClass_Id = BudgetClass_Id
            };
        }
    }
}
