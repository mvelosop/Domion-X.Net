using DFlow.Tennants.Lib.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace DFlow.Tennants.Setup
{
    public class TennantsDbSetupHelper
    {
        //private static string _defaultConnectionString = "Data Source=localhost;Initial Catalog=DFlow.Tennants.Lib.Tests;Integrated Security=SSPI;MultipleActiveResultSets=true";

        private string _connectionString;
        private DbContextOptions<TennantsDbContext> _options;

        public TennantsDbSetupHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public TennantsDbContext GetDbContext()
        {
            if (_options == null) throw new InvalidOperationException($"Must run {nameof(TennantsDbSetupHelper)}.{nameof(SetupDatabase)} first!");

            return new TennantsDbContext(_options);
        }

        public DbContextOptions<TennantsDbContext> GetOptions()
        {
            var optionBuilder = new DbContextOptionsBuilder<TennantsDbContext>();

            optionBuilder.UseSqlServer(_connectionString);

            return optionBuilder.Options;
        }

        public void SetupDatabase()
        {
            lock (_connectionString)
            {
                _options = GetOptions();

                using (var dbContext = GetDbContext())
                {
                    dbContext.Database.Migrate();
                }
            }
        }
    }
}
