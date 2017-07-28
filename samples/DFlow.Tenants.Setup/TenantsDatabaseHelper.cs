using DFlow.Budget.Lib.Data;
using DFlow.Tenants.Lib.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace DFlow.Tenants.Setup
{
    public class TenantsDatabaseHelper
    {
        private readonly string _connectionString;

        private DbContextOptions<TenantsDbContext> _options;

        public TenantsDatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Returns the DbContext if the database has been set up.
        /// </summary>
        /// <returns></returns>
        public TenantsDbContext CreateDbContext()
        {
            if (_options == null) throw new InvalidOperationException($"Must run {nameof(TenantsDatabaseHelper)}.{nameof(ConfigureDatabase)} first!");

            return new TenantsDbContext(_options);
        }

        /// <summary>
        /// Creates the database and applies pending migrations.
        /// </summary>
        public void ConfigureDatabase()
        {
            var optionBuilder = new DbContextOptionsBuilder<TenantsDbContext>();

            lock (_connectionString)
            {
                optionBuilder.UseSqlServer(_connectionString);
                //optionBuilder.EnableSensitiveDataLogging();

                _options = optionBuilder.Options;

                using (var dbContext = CreateDbContext())
                {
                    dbContext.Database.Migrate();
                }
            }
        }
    }
}
