using DFlow.Tennants.Lib.Data;
using DFlow.Tennants.Setup;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Demo.Cli
{
    public class TennantsDbContextFactory : IDbContextFactory<TennantsDbContext>
    {
        private static string _connectionString = "Data Source=localhost;Initial Catalog=DFlow.Tennants.Lib.Tests;Integrated Security=SSPI;MultipleActiveResultSets=true";

        public TennantsDbContext Create(DbContextFactoryOptions options)
        {
            var dbSetup = new TennantsDbSetupHelper(_connectionString);

            return new TennantsDbContext(dbSetup.GetOptions());
        }
    }
}
