using System;
using Autofac;
using DFlow.Budget.Lib.Services;
using DFlow.Budget.Lib.Tests.Helpers;
using DFlow.Budget.Setup;
using Domion.FluentAssertions.Extensions;
using FluentAssertions;
using Xunit;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace DFlow.Budget.Lib.Tests
{
    [Trait("Type", "Integration")]
    public class BudgetClassManager_IntegrationTests
    {
        private static string _connectionString = "Data Source=localhost;Initial Catalog=DFlow.Budget.Lib.Tests;Integrated Security=SSPI;MultipleActiveResultSets=true";

        private IContainer _container;

        public BudgetClassManager_IntegrationTests()
        {
            BudgetDbSetupHelper dbHelper = SetupDatabase(_connectionString);

            _container = SetupContainer(dbHelper);
        }

        [Fact]
        public void TryDelete_DeletesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new BudgetClassData("Delete test - Inserted", "Income");

            EnsureEntitiesExist(data);

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            using (var scope = GetLocalScope())
            {
                var manager = scope.Resolve<BudgetClassManager>();

                var entity = manager.GetEntityByKeyValue(data.Name);

                errors = manager.TryDelete(entity);

                manager.SaveChanges();
            }

            // Assert ----------------------------

            errors.Should().BeEmpty();

            AssertEntitiesDoNotExist(data);
        }

        [Fact]
        public void TryInsert_Fails_WhenDuplicateData()
        {
            // Arrange ---------------------------

            var data = new BudgetClassData("Duplicate Insert test - Inserted", "Income");

            EnsureEntitiesExist(data);

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            using (var scope = GetLocalScope())
            {
                var manager = scope.Resolve<BudgetClassManager>();

                var entity = data.CreateEntity();

                errors = manager.TryInsert(entity).ToList();
            }

            // Assert ----------------------------

            errors.Should().ContainErrorMessage(BudgetClassManager.duplicateByNameError);
        }

        [Fact]
        public void TryInsert_InsertsRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new BudgetClassData("Insert test - Inserted", "Income");

            EnsureEntitiesDoNotExist(data);

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            using (var scope = GetLocalScope())
            {
                var manager = scope.Resolve<BudgetClassManager>();

                var entity = data.CreateEntity();

                errors = manager.TryInsert(entity);

                manager.SaveChanges();
            }

            // Assert ----------------------------

            errors.Should().BeEmpty();

            AssertEntitiesExist(data);
        }

        [Fact]
        public void TryUpdate_Fails_WhenDuplicateData()
        {
            // Arrange ---------------------------

            var dataFirst = new BudgetClassData("Duplicate Update test - Inserted first", "Income");
            var dataSecond = new BudgetClassData("Duplicate Update test - Inserted second", "Income");

            EnsureEntitiesExist(dataFirst, dataSecond);

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            using (var scope = GetLocalScope())
            {
                var manager = scope.Resolve<BudgetClassManager>();

                var entity = manager.GetEntityByKeyValue(dataFirst.Name);

                entity.Name = dataSecond.Name;

                errors = manager.TryUpdate(entity).ToList();
            }

            // Assert ----------------------------

            errors.Should().ContainErrorMessage(BudgetClassManager.duplicateByNameError);
        }

        [Fact]
        public void TryUpdate_UpdatesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new BudgetClassData("Update test - Inserted", "Income");
            var update = new BudgetClassData("Update test - UPDATED", "Income");

            EnsureEntitiesExist(data);
            EnsureEntitiesDoNotExist(update);

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            using (var scope = GetLocalScope())
            {
                var manager = scope.Resolve<BudgetClassManager>();

                var entity = manager.GetEntityByKeyValue(data.Name);

                entity.Name = update.Name;

                errors = manager.TryUpdate(entity);

                manager.SaveChanges();
            }

            // Assert ----------------------------

            errors.Should().BeEmpty();

            AssertEntitiesExist(update);
        }

        private void AssertEntitiesDoNotExist(params BudgetClassData[] data)
        {
            using (var scope = GetLocalScope())
            {
                var managerHelper = scope.Resolve<BudgetClassManagerHelper>();

                managerHelper.AssertEntitiesDoNotExist(data);
            }
        }

        private void AssertEntitiesExist(params BudgetClassData[] data)
        {
            using (var scope = GetLocalScope())
            {
                var managerHelper = scope.Resolve<BudgetClassManagerHelper>();

                managerHelper.AssertEntitiesExist(data);
            }
        }

        private void EnsureEntitiesDoNotExist(params BudgetClassData[] data)
        {
            using (var scope = GetLocalScope())
            {
                var managerHelper = scope.Resolve<BudgetClassManagerHelper>();

                managerHelper.EnsureEntitiesDoNotExist(data);
            }
        }

        private void EnsureEntitiesExist(params BudgetClassData[] data)
        {
            using (var scope = GetLocalScope())
            {
                var managerHelper = scope.Resolve<BudgetClassManagerHelper>();

                managerHelper.EnsureEntitiesExist(data);
            }
        }

        private ILifetimeScope GetLocalScope(IContainer scope = null)
        {
            IContainer container = scope ?? _container;

            return container.BeginLifetimeScope();
        }

        private IContainer SetupContainer(BudgetDbSetupHelper dbHelper)
        {
            var autofacHelper = new BudgetAutofacSetupHelper(dbHelper);

            var builder = new ContainerBuilder();

            autofacHelper.SetupContainer(builder);

            IContainer container = builder.Build();

            return container;
        }

        private BudgetDbSetupHelper SetupDatabase(string connectionString)
        {
            BudgetDbSetupHelper dbHelper = new BudgetDbSetupHelper(_connectionString);

            dbHelper.SetupDatabase();

            return dbHelper;
        }
    }
}
