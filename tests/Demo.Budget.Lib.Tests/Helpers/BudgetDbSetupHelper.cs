using Demo.Budget.Lib.Data;
using Microsoft.EntityFrameworkCore;

namespace Demo.Budget.Lib.Tests.Helpers
{
    public class BudgetDbSetupHelper
    {
        private static string _connectionString = "Data Source=localhost;Initial Catalog=Demo.Budget.Lib.Tests;Integrated Security=SSPI;MultipleActiveResultSets=true";
        private static DbContextOptions<BudgetDbContext> _options;

        static BudgetDbSetupHelper()
        {
            var optionBuilder = new DbContextOptionsBuilder<BudgetDbContext>();

            optionBuilder.UseSqlServer(_connectionString);

            _options = optionBuilder.Options;
        }

        public BudgetDbSetupHelper()
        {
            SetupDatabase();
        }

        public BudgetDbContext GetDbContext()
        {
            return new BudgetDbContext(_options);
        }

        private void SetupDatabase()
        {
            using (var dbContext = GetDbContext())
            {
                dbContext.Database.Migrate();
            }
        }
    }
}
