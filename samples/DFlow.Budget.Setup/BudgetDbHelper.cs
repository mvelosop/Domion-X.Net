using DFlow.Budget.Lib.Data;
using DFlow.Tenants.Setup;
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

        public TenantsDbHelper TenantsDbHelper { get; private set; }

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
            SetupExternalDatabases();

            var optionBuilder = new DbContextOptionsBuilder<BudgetDbContext>();

            optionBuilder.UseSqlServer(ConnectionString);

            _options = optionBuilder.Options;

            using (var dbContext = CreateDbContext())
            {
                dbContext.Database.Migrate();
            }
        }

        private void SetupExternalDatabases()
        {
            TenantsDbHelper = new TenantsDbHelper(ConnectionString);

            TenantsDbHelper.SetupDatabase();
        }
    }
}
