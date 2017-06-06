using DFlow.Budget.Core.Model;
using System;

namespace DFlow.Budget.Lib.Tests.Helpers
{
    public class BudgetClassData
    {
        public BudgetClassData()
        {
        }

        public BudgetClassData(BudgetClass entity)
        {
            Name = entity.Name;

            TransactionType = entity.TransactionType.ToString();
        }

        public BudgetClassData(string name, string transactionType)
        {
            Name = name;

            TransactionType = transactionType;
        }

        public string Name { get; set; }

        public string TransactionType { get; set; }

        public BudgetClass CreateEntity()
        {
            return new BudgetClass
            {
                Name = Name,
                TransactionType = (TransactionType)Enum.Parse(typeof(TransactionType), TransactionType, true)
            };
        }
    }
}
