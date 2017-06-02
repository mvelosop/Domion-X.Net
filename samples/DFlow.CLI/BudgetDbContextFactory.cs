using DFlow.Budget.Lib.Data;
using DFlow.Budget.Lib.Tests.Helpers;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Demo.Cli
{
    public class BudgetDbContextFactory : IDbContextFactory<BudgetDbContext>
    {
        public BudgetDbContext Create(DbContextFactoryOptions options)
        {
            var dbSetup = new BudgetDbSetupHelper();

            return dbSetup.GetDbContext();
        }
    }
}
