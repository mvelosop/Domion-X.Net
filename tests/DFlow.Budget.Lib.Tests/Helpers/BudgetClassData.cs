﻿using DFlow.Budget.Core.Model;
using System;
using System.Linq.Expressions;

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
        }

        public BudgetClassData(string name)
        {
            Name = name;
        }

        public Expression<Func<BudgetClass, bool>> KeyExpression => bc => bc.Name == Name;

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