using DFlow.Budget.Lib.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace DFlow.Budget.Setup
{
    public class BudgetDbSetupHelper
    {
        //private static string _defaultConnectionString = "Data Source=localhost;Initial Catalog=DFlow.Budget.Lib.Tests;Integrated Security=SSPI;MultipleActiveResultSets=true";

        private string _connectionString;
        private DbContextOptions<BudgetDbContext> _options;

        public BudgetDbSetupHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public BudgetDbContext GetDbContext()
        {
            if (_options == null) throw new InvalidOperationException($"Must run {nameof(BudgetDbSetupHelper)}.{nameof(SetupDatabase)} first!");

            return new BudgetDbContext(_options);
        }

        public void SetupDatabase()
        {
            lock (_connectionString)
            {
                var optionBuilder = new DbContextOptionsBuilder<BudgetDbContext>();

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
