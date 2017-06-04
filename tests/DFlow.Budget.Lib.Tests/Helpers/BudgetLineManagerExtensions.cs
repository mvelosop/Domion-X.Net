using DFlow.Budget.Core.Model;
using DFlow.Budget.Lib.Services;
using FluentAssertions;

namespace DFlow.Budget.Lib.Tests.Helpers
{
    public static class BudgetLineManagerExtensions
    {
        /// <summary>
        /// Gets the entity by key data value or fails the assertion.
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="name">Key data value</param>
        /// <returns>The entity </returns>
        public static BudgetLine GetByKeyDataValue(this BudgetLineManager manager, string name)
        {
            BudgetLine entity = manager.SingleOrDefault(c => c.Name == name);

            entity.Should().NotBeNull(@"because BudgetLine ""{0}"" MUST EXIST!", name);

            return entity;
        }
    }
}
