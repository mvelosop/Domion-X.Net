using DFlow.Tenants.Lib.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DFlow.CLI
{
    public class TenantDbContextFactory : IDbContextFactory<TenantsDbContext>
    {
        public TenantsDbContext Create(DbContextFactoryOptions options)
        {
            var optionBuilder = new DbContextOptionsBuilder<TenantsDbContext>();

            optionBuilder.UseSqlServer("x");

            return new TenantsDbContext(optionBuilder.Options);
        }
    }
}
