using Demo.Budget.Lib.Data;
using Microsoft.EntityFrameworkCore;

namespace Demo.Budget.Lib.Tests.Helpers
{
    public class BudgetDbSetupHelper
    {
        private static string _defaultConnectionString = "Data Source=localhost;Initial Catalog=Demo.Budget.Lib.Tests;Integrated Security=SSPI;MultipleActiveResultSets=true";

        private DbContextOptions<BudgetDbContext> _options;

        public BudgetDbSetupHelper()
            : this(_defaultConnectionString)
        {
        }

        public BudgetDbSetupHelper(string connectionString)
        {
            var optionBuilder = new DbContextOptionsBuilder<BudgetDbContext>();

            optionBuilder.UseSqlServer(connectionString);

            _options = optionBuilder.Options;

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
