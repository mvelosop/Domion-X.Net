using DFlow.Budget.Lib.Tests.Helpers;
using DFlow.Transactions.Lib.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace DFlow.Transactions.Lib.Tests.Helpers
{
    public class TransactionsDbSetupHelper
    {
        private static string _defaultConnectionString = "Data Source=localhost;Initial Catalog=DFlow.Transactions.Lib.Tests;Integrated Security=SSPI;MultipleActiveResultSets=true";

        private string _connectionString;
        private DbContextOptions<TransactionsDbContext> _options;

        public TransactionsDbSetupHelper()
            : this(_defaultConnectionString)
        {
        }

        public TransactionsDbSetupHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public TransactionsDbContext GetDbContext()
        {
            if (_options == null) throw new InvalidOperationException($"Must run {nameof(TransactionsDbSetupHelper)}.{nameof(SetupDatabase)} first!");

            return new TransactionsDbContext(_options);
        }

        public void SetupDatabase()
        {
            var budgetDbHelper = new BudgetDbSetupHelper(_connectionString);

            budgetDbHelper.SetupDatabase();

            lock (_connectionString)
            {
                var optionBuilder = new DbContextOptionsBuilder<TransactionsDbContext>();

                optionBuilder.UseSqlServer(_connectionString);

                _options = optionBuilder.Options;

                using (var dbContext = GetDbContext())
                {
                    dbContext.Database.Migrate();
                }
            }
        }
    }
}
