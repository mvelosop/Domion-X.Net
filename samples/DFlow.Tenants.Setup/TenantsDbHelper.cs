using DFlow.Budget.Lib.Data;
using DFlow.Tenants.Lib.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace DFlow.Tenants.Setup
{
    public class TenantsDbHelper
    {
        private readonly string ConnectionString;

        private DbContextOptions<TenantsDbContext> _options;

        public TenantsDbHelper(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Returns the DbContext if the database has been set up.
        /// </summary>
        /// <returns></returns>
        public TenantsDbContext CreateDbContext()
        {
            if (_options == null) throw new InvalidOperationException($"Must run {nameof(TenantsDbHelper)}.{nameof(SetupDatabase)} first!");

            return new TenantsDbContext(_options);
        }

        /// <summary>
        /// Creates the database and applies pending migrations.
        /// </summary>
        public void SetupDatabase()
        {
            var optionBuilder = new DbContextOptionsBuilder<TenantsDbContext>();

            optionBuilder.UseSqlServer(ConnectionString);

            _options = optionBuilder.Options;

            using (var dbContext = CreateDbContext())
            {
                dbContext.Database.Migrate();
            }
        }
    }
}
