using DFlow.Tenants.Lib.Data;
using DFlow.Tenants.Setup;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Demo.Cli
{
    public class TenantsDbContextFactory : IDbContextFactory<TenantsDbContext>
    {
        private static string _connectionString = "Data Source=localhost;Initial Catalog=DFlow.Tenants.Lib.Tests;Integrated Security=SSPI;MultipleActiveResultSets=true";

        public TenantsDbContext Create(DbContextFactoryOptions options)
        {
            var dbSetup = new TenantsDbSetupHelper(_connectionString);

            return new TenantsDbContext(dbSetup.GetOptions());
        }
    }
}
