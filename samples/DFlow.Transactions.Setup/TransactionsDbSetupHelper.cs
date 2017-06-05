using DFlow.Budget.Setup;
using DFlow.Transactions.Lib.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace DFlow.Transactions.Setup
{
    public class TransactionsDbSetupHelper
    {
        //private static string _defaultConnectionString = "Data Source=localhost;Initial Catalog=DFlow.Budget.Lib.Tests;Integrated Security=SSPI;MultipleActiveResultSets=true";

        private string _connectionString;
        private DbContextOptions<TransactionsDbContext> _options;

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
            BudgetDbSetupHelper dbHelper = new BudgetDbSetupHelper(_connectionString);

            dbHelper.SetupDatabase();

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
