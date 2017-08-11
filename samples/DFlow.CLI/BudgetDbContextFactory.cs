using DFlow.Budget.Lib.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DFlow.CLI
{
    public class BudgetDbContextFactory : IDbContextFactory<BudgetDbContext>
    {
        public BudgetDbContext Create(DbContextFactoryOptions options)
        {
            var optionBuilder = new DbContextOptionsBuilder<BudgetDbContext>();

            optionBuilder.UseSqlServer("x");

            return new BudgetDbContext(optionBuilder.Options);
        }
    }
}
