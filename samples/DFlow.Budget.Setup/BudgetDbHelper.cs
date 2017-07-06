using DFlow.Budget.Lib.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace DFlow.Budget.Setup
{
    public class BudgetDbHelper
    {
        private readonly string ConnectionString;

        private DbContextOptions<BudgetDbContext> _options;

        public BudgetDbHelper(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Returns the DbContext if the database has been set up.
        /// </summary>
        /// <returns></returns>
        public BudgetDbContext CreateDbContext()
        {
            if (_options == null) throw new InvalidOperationException($"Must run {nameof(BudgetDbHelper)}.{nameof(SetupDatabase)} first!");

            return new BudgetDbContext(_options);
        }

        /// <summary>
        /// Creates the database and applies pending migrations.
        /// </summary>
        public void SetupDatabase()
        {
            var optionBuilder = new DbContextOptionsBuilder<BudgetDbContext>();

            optionBuilder.UseSqlServer(ConnectionString);

            _options = optionBuilder.Options;

            using (var dbContext = CreateDbContext())
            {
                dbContext.Database.Migrate();
            }
        }
    }
}
