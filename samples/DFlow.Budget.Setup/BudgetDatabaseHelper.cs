using DFlow.Budget.Lib.Data;
using DFlow.Tenants.Setup;
using Microsoft.EntityFrameworkCore;
using System;

namespace DFlow.Budget.Setup
{
    public class BudgetDatabaseHelper
    {
        private readonly string _connectionString;

        private DbContextOptions<BudgetDbContext> _options;

        public BudgetDatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public TenantsDatabaseHelper TenantsDbHelper { get; private set; }

        /// <summary>
        /// Returns the DbContext if the database has been set up.
        /// </summary>
        /// <returns></returns>
        public BudgetDbContext CreateDbContext()
        {
            if (_options == null) throw new InvalidOperationException($"Must run {nameof(BudgetDatabaseHelper)}.{nameof(ConfigureDatabase)} first!");

            return new BudgetDbContext(_options);
        }

        /// <summary>
        /// Creates the database and applies pending migrations.
        /// </summary>
        public void ConfigureDatabase()
        {
            ConfigureExternalDatabases();

            var optionBuilder = new DbContextOptionsBuilder<BudgetDbContext>();

            lock (_connectionString)
            {
                optionBuilder.UseSqlServer(_connectionString);

                _options = optionBuilder.Options;

                using (var dbContext = CreateDbContext())
                {
                    dbContext.Database.Migrate();
                }
            }
        }

        private void ConfigureExternalDatabases()
        {
            TenantsDbHelper = new TenantsDatabaseHelper(_connectionString);

            TenantsDbHelper.ConfigureDatabase();
        }
    }
}
