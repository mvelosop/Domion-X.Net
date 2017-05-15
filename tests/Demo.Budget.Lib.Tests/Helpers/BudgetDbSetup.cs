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

        public static BudgetDbContext GetDbContext()
        {
            return new BudgetDbContext(_options);
        }

        public void AssertDelete(BudgetClass entity)
        {
            var errors = TryDelete(entity);

            errors.Should().BeEmpty();
        }

        public void AssertEntitiesDoNotExist(params BudgetClassData[] dataArray)
        {
            using (var dbContext = GetDbContext())
            {
                var manager = new BudgetClassManager(dbContext);

                foreach (var data in dataArray)
                {
                    var entities = manager.Query(ci => ci.Name == data.Name).ToList();

                    foreach (var entity in entities)
                    {
                        var errors = manager.TryDelete(entity);

                        errors.Should().BeEmpty();
                    }
                }

                manager.SaveChanges();
            }

            using (var dbContext = GetDbContext())
            {
                var manager = new BudgetClassManager(dbContext);

                foreach (var data in dataArray)
                {
                    var entities = manager.Query(ci => ci.Name == data.Name).ToList();

                    entities.Should().BeEmpty();
                }
            }
        }

        public void AssertInsert(BudgetClass entity)
        {
            var errors = TryInsert(entity);

            errors.Should().BeEmpty();
        }

        public void AssertUpdate(BudgetClass entity)
        {
            var errors = TryUpdate(entity);

            errors.Should().BeEmpty();
        }

        public BudgetClass GetEntity(BudgetClassData data)
        {
            using (var dbContext = GetDbContext())
            {
                var manager = new BudgetClassManager(dbContext);

                return manager.SingleOrDefault(ci => ci.Name == data.Name);
            }
        }

        public IEnumerable<ValidationResult> TryDelete(BudgetClass entity)
        {
            using (var dbContext = GetDbContext())
            {
                var manager = new BudgetClassManager(dbContext);

                var errors = manager.TryDelete(entity);

                if (!errors.Any())
                {
                    manager.SaveChanges();
                }

                return errors.ToList();
            }
        }

        public IEnumerable<ValidationResult> TryInsert(BudgetClass entity)
        {
            using (var dbContext = GetDbContext())
            {
                var manager = new BudgetClassManager(dbContext);

                var errors = manager.TryInsert(entity);

                if (!errors.Any())
                {
                    manager.SaveChanges();
                }

                return errors.ToList();
            }
        }

        public IEnumerable<ValidationResult> TryUpdate(BudgetClass entity)
        {
            using (var dbContext = GetDbContext())
            {
                var manager = new BudgetClassManager(dbContext);

                var errors = manager.TryUpdate(entity);

                if (!errors.Any())
                {
                    manager.SaveChanges();
                }

                return errors.ToList();
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
