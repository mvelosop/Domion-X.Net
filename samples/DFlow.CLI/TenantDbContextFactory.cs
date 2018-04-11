using DFlow.Tenants.Lib.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DFlow.CLI
{
    public class TenantDbContextFactory : IDesignTimeDbContextFactory<TenantsDbContext>
    {
        public TenantsDbContext CreateDbContext(string[] args)
        {
            var optionBuilder = new DbContextOptionsBuilder<TenantsDbContext>();

            optionBuilder.UseSqlServer("x");

            return new TenantsDbContext(optionBuilder.Options);
        }
    }
}
