using Demo.Budget.Lib.Tests.Helpers;
using Demo.Transactions.Lib.Data;
using Microsoft.EntityFrameworkCore;

namespace Demo.Transactions.Lib.Tests.Helpers
{
    public class TransactionsDbSetupHelper
    {
        private static string _defaultConnectionString = "Data Source=localhost;Initial Catalog=Demo.Transactions.Lib.Tests;Integrated Security=SSPI;MultipleActiveResultSets=true";

        private string _connectionString;
        private DbContextOptions<TransactionsDbContext> _options;

        public TransactionsDbSetupHelper()
            : this(_defaultConnectionString)
        {
        }

        public TransactionsDbSetupHelper(string connectionString)
        {
            _connectionString = connectionString;

            var optionBuilder = new DbContextOptionsBuilder<TransactionsDbContext>();

            optionBuilder.UseSqlServer(_connectionString);

            _options = optionBuilder.Options;

            DatabaseSetup();
        }

        public TransactionsDbContext GetDbContext()
        {
            return new TransactionsDbContext(_options);
        }

        private void DatabaseSetup()
        {
            var dbSetupHelper = new BudgetDbSetupHelper(_connectionString);

            using (var dbContext = GetDbContext())
            {
                dbContext.Database.Migrate();
            }
        }
    }
}
