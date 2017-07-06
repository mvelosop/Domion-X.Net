using DFlow.WebApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DFlow.CLI
{
    public class ApplicationDbContextFactory : IDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext Create(DbContextFactoryOptions options)
        {
            var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            optionBuilder.UseSqlServer("x");

            return new ApplicationDbContext(optionBuilder.Options);
        }
    }
}
