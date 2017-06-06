using DFlow.Budget.Lib.Data;
using Domion.Setup;
using Microsoft.EntityFrameworkCore;

namespace DFlow.Budget.Setup
{
    public class BudgetDbSetupHelper : DbSetupHelper<BudgetDbContext>
    {
        public BudgetDbSetupHelper(string connectionString)
            : base(connectionString)
        {
        }

        protected override BudgetDbContext CreateRawDbContext(DbContextOptions<BudgetDbContext> options)
        {
            return new BudgetDbContext(options);
        }
    }
}
