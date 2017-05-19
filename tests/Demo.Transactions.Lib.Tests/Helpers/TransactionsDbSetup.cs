using Demo.Transactions.Lib.Data;
using Microsoft.EntityFrameworkCore;

namespace Demo.Transactions.Lib.Tests.Helpers
{
    public class TransactionsDbSetup
    {
        private static string _connectionString = "Data Source=localhost;Initial Catalog=Demo.Transactions.Lib.Tests;Integrated Security=SSPI;MultipleActiveResultSets=true";
        private static DbContextOptions<TransactionsDbContext> _options;

        static TransactionsDbSetup()
        {
            var optionBuilder = new DbContextOptionsBuilder<TransactionsDbContext>();

            optionBuilder.UseSqlServer(_connectionString);

            _options = optionBuilder.Options;
        }

        public TransactionsDbSetup()
        {
            DatabaseSetup();
        }

        public TransactionsDbContext GetDbContext()
        {
            return new TransactionsDbContext(_options);
        }

        private void DatabaseSetup()
        {
            using (var dbContext = GetDbContext())
            {
                dbContext.Database.Migrate();
            }
        }
    }
}
