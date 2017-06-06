using DFlow.Budget.Lib.Data;
using DFlow.Budget.Setup;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Demo.Cli
{
    public class BudgetDbContextFactory : IDbContextFactory<BudgetDbContext>
    {
        private static string _connectionString = "Data Source=localhost;Initial Catalog=DFlow.Budget.Lib.Tests;Integrated Security=SSPI;MultipleActiveResultSets=true";

        public BudgetDbContext Create(DbContextFactoryOptions options)
        {
            var dbSetup = new BudgetDbSetupHelper(_connectionString);

            return dbSetup.GetDbContext();
        }
    }
}
