using DFlow.Transactions.Lib.Data;
using DFlow.Transactions.Setup;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Demo.Cli
{
    public class TransactionsDbContextFactory : IDbContextFactory<TransactionsDbContext>
    {
        private static string _connectionString = "Data Source=localhost;Initial Catalog=DFlow.Tennants.Lib.Tests;Integrated Security=SSPI;MultipleActiveResultSets=true";

        public TransactionsDbContext Create(DbContextFactoryOptions options)
        {
            var dbSetup = new TransactionsDbSetupHelper(_connectionString);

            return dbSetup.CreateDbContext();
        }
    }
}
