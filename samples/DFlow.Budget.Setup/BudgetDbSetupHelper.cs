using DFlow.Budget.Lib.Data;
using DFlow.Tennants.Setup;
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

        public override void SetupDatabase()
        {
            var tennantsDbHelper = new TennantsDbSetupHelper(ConnectionString);

            tennantsDbHelper.SetupDatabase();

            base.SetupDatabase();
        }

        protected override BudgetDbContext CreateRawDbContext(DbContextOptions<BudgetDbContext> options)
        {
            return new BudgetDbContext(options);
        }
    }
}
