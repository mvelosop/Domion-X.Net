using DFlow.Budget.Core.Model;
using DFlow.Budget.Lib.Services;
using FluentAssertions;

namespace DFlow.Budget.Lib.Tests.Helpers
{
    public static class BudgetClassManagerExtensions
    {
        /// <summary>
        /// Gets the entity by key data value
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="name">Key data value</param>
        /// <returns></returns>
        public static BudgetClass GetByKeyDataValue(this BudgetClassManager manager, string name)
        {
            BudgetClass entity = manager.SingleOrDefault(c => c.Name == name);

            return entity;
        }
    }
}
