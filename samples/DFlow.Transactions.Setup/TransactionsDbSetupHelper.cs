using DFlow.Budget.Setup;
using DFlow.Transactions.Lib.Data;
using Domion.Setup;
using Microsoft.EntityFrameworkCore;

namespace DFlow.Transactions.Setup
{
    public class TransactionsDbSetupHelper : DbSetupHelper<TransactionsDbContext> 
    {
        public TransactionsDbSetupHelper(string connectionString)
            : base(connectionString)
        {
        }

        public override void SetupDatabase()
        {
            var budgetDbHelper = new BudgetDbSetupHelper(ConnectionString);

            budgetDbHelper.SetupDatabase();

            base.SetupDatabase();
        }

        protected override TransactionsDbContext CreateRawDbContext(DbContextOptions<TransactionsDbContext> options)
        {
            return new TransactionsDbContext(options);
        }
    }
}
