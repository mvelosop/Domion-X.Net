using DFlow.Tenants.Lib.Data;
using Domion.Setup;
using Microsoft.EntityFrameworkCore;

namespace DFlow.Tenants.Setup
{
    public class TenantsDbSetupHelper : DbSetupHelper<TenantsDbContext>
    {
        public TenantsDbSetupHelper(string connectionString)
            : base(connectionString)
        {
        }

        protected override TenantsDbContext CreateRawDbContext(DbContextOptions<TenantsDbContext> options)
        {
            return new TenantsDbContext(options);
        }
    }
}
