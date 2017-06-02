using DFlow.Budget.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

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

        public int BudgetClass_Id { get; set; }

        public string ClassName { get; set; }

        public Expression<Func<BudgetLine, bool>> KeyExpression => bc => bc.Name == Name;

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
