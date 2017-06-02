using DFlow.Transactions.Lib.Data;
using DFlow.Transactions.Lib.Tests.Helpers;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Demo.Cli
{
    public class TransactionsDbContextFactory : IDbContextFactory<TransactionsDbContext>
    {
        public TransactionsDbContext Create(DbContextFactoryOptions options)
        {
            var dbSetup = new TransactionsDbSetupHelper();

            return dbSetup.GetDbContext();
        }
    }
}
