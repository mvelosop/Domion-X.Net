using Demo.Budget.Core.Model;
using Demo.Budget.Lib.Data;
using Demo.Budget.Lib.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Demo.Budget.Lib.Tests.Helpers
{
    public class BudgetDbSetup
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
            DatabaseSetup();
        }

        public BudgetDbContext GetDbContext()
        {
            return new BudgetDbContext(_options);
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
