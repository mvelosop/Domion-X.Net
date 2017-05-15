using Demo.Budget.Core.Model;
using Demo.Budget.Lib.Data;
using Demo.Budget.Lib.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

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

        public static BudgetDbContext GetDbContext()
        {
            return new BudgetDbContext(_options);
        }

        public void AssertEntitiesDoesNotExist(params BudgetClassData[] dataArray)
        {
            foreach (var data in dataArray)
            {
                var entity = GetEntity(data);

                if (entity != null)
                {
                    TryDelete(entity);

                    entity = GetEntity(data);

                    entity.Should().BeNull();
                }
            }
        }

        public BudgetClass GetEntity(BudgetClassData data)
        {
            using (var dbContext = GetDbContext())
            {
                var manager = new BudgetClassManager(dbContext);

                return manager.SingleOrDefault(ci => ci.Name == data.Name);
            }
        }

        public void TryDelete(BudgetClass entity)
        {
            using (var dbContext = GetDbContext())
            {
                var manager = new BudgetClassManager(dbContext);

                var errors = manager.TryDelete(entity);

                errors.Should().BeEmpty();

                manager.SaveChanges();
            }
        }

        public void TryInsert(BudgetClass item)
        {
            using (var dbContext = GetDbContext())
            {
                var manager = new BudgetClassManager(dbContext);

                var errors = manager.TryInsert(item);

                errors.Should().BeEmpty();

                manager.SaveChanges();
            }
        }

        public void TryUpdate(BudgetClass item)
        {
            using (var dbContext = GetDbContext())
            {
                var manager = new BudgetClassManager(dbContext);

                var errors = manager.TryUpdate(item);

                errors.Should().BeEmpty();

                manager.SaveChanges();
            }
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
