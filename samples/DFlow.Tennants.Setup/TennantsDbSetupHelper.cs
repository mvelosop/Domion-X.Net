using DFlow.Tennants.Lib.Data;
using Domion.Setup;
using Microsoft.EntityFrameworkCore;

namespace DFlow.Tennants.Setup
{
    public class TennantsDbSetupHelper : DbSetupHelper<TennantsDbContext>
    {
        public TennantsDbSetupHelper(string connectionString)
            : base(connectionString)
        {
        }

        protected override TennantsDbContext CreateRawDbContext(DbContextOptions<TennantsDbContext> options)
        {
            return new TennantsDbContext(options);
        }
    }
}
