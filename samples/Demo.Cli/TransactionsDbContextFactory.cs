using Demo.Budget.Lib.Data;
using Demo.Budget.Lib.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Cli
{
    public class BudgetDbContextFactory : IDbContextFactory<BudgetDbContext>
    {
        public BudgetDbContext Create(DbContextFactoryOptions options)
        {
            var dbSetup = new BudgetDbSetup();

            return dbSetup.GetDbContext();
        }
    }
}
