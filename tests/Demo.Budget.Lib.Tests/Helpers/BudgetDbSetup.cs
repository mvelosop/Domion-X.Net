using Demo.Budget.Lib.Data;
using Domion.FluentAssertions.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Demo.Budget.Lib.Tests.Helpers
{
    public class BudgetDbSetup : BaseDbSetup<BudgetDbContext>
    {
        private static string _connectionString = "Data Source=localhost;Initial Catalog=Demo.Budget.Lib.Tests;Integrated Security=SSPI;MultipleActiveResultSets=true";
        private static DbContextOptions<BudgetDbContext> _options;

        static BudgetDbSetup()
        {
            var optionBuilder = new DbContextOptionsBuilder<BudgetDbContext>();

            optionBuilder.UseSqlServer(_connectionString);

            _options = optionBuilder.Options;
        }

        public BudgetDbSetup()
        {
            SetupDatabase();
        }

        public override BudgetDbContext GetDbContext()
        {
            return new BudgetDbContext(_options);
        }

    }
}
