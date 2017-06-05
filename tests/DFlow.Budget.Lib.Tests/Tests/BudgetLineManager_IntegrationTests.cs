using Autofac;
using DFlow.Budget.Lib.Services;
using DFlow.Budget.Lib.Tests.Helpers;
using DFlow.Budget.Setup;
using Domion.FluentAssertions.Extensions;
using FluentAssertions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace DFlow.Budget.Lib.Tests
{
    [Trait("Type", "Integration")]
    public class BudgetLineManager_IntegrationTests
    {
        private static string _connectionString = "Data Source=localhost;Initial Catalog=DFlow.Budget.Lib.Tests;Integrated Security=SSPI;MultipleActiveResultSets=true";

        private IContainer _container;

        public BudgetLineManager_IntegrationTests()
        {
            BudgetDbSetupHelper dbHelper = SetupDatabase(_connectionString);

            _container = SetupContainer(dbHelper);

            using (var scope = GetLocalScope())
            {
                var budgetClassHelper = scope.Resolve<BudgetClassManagerHelper>();

                BudgetClassData[] dataSet = new BudgetClassData[]
                {
                    new BudgetClassData("Data-BudgetClass", "Income"),
                };

                budgetClassHelper.EnsureEntitiesExist(dataSet);
            }
        }

        [Fact]
        public void TryDelete_DeletesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new BudgetLineData("Delete-Success-Valid - Inserted", "Data-BudgetClass");

            EnsureEntitiesExist(data);

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            using (var scope = GetLocalScope())
            {
                var manager = scope.Resolve<BudgetLineManager>();

                var entity = manager.AssertGetByKeyData(data.Name);

                errors = manager.TryDelete(entity);

                manager.SaveChanges();
            }

            // Assert ----------------------------

            errors.Should().BeEmpty();

            AssertEntitiesDoNotExist(data);
        }

        [Fact]
        public void TryInsert_Fails_WhenDuplicateKeyData()
        {
            // Arrange ---------------------------

            var data = new BudgetLineData("Insert-Error-Duplicate - Inserted", "Data-BudgetClass");

            EnsureEntitiesExist(data);

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            using (var scope = GetLocalScope())
            {
                var manager = scope.Resolve<BudgetLineManager>();

                var entity = data.CreateEntity();

                errors = manager.TryInsert(entity).ToList();
            }

            // Assert ----------------------------

            errors.Should().ContainErrorMessage(BudgetLineManager.duplicateByNameError);
        }

        [Fact]
        public void TryInsert_InsertsRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new BudgetLineData("Insert-Success-Valid - Inserted", "Data-BudgetClass");

            EnsureEntitiesDoNotExist(data);

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            using (var scope = GetLocalScope())
            {
                var manager = scope.Resolve<BudgetLineManager>();
                var dataHelper = scope.Resolve<BudgetLineDataHelper>();

                dataHelper.SetReferences(data);

                var entity = data.CreateEntity();

                errors = manager.TryInsert(entity);

                manager.SaveChanges();
            }

            // Assert ----------------------------

            errors.Should().BeEmpty();

            AssertEntitiesExist(data);
        }

        [Fact]
        public void TryUpdate_Fails_WhenDuplicateKeyData()
        {
            // Arrange ---------------------------

            var dataFirst = new BudgetLineData("Update-Error-Duplicate - Inserted first", "Data-BudgetClass");
            var dataSecond = new BudgetLineData("Update-Error-Duplicate - Inserted second", "Data-BudgetClass");

            EnsureEntitiesExist(dataFirst, dataSecond);

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            using (var scope = GetLocalScope())
            {
                var manager = scope.Resolve<BudgetLineManager>();

                var entity = manager.AssertGetByKeyData(dataFirst.Name);

                entity.Name = dataSecond.Name;

                errors = manager.TryUpdate(entity).ToList();
            }

            // Assert ----------------------------

            errors.Should().ContainErrorMessage(BudgetLineManager.duplicateByNameError);
        }

        [Fact]
        public void TryUpdate_UpdatesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new BudgetLineData("Update-Success-Valid - Inserted", "Data-BudgetClass");
            var update = new BudgetLineData("Update-Success-Valid - Updated", "Data-BudgetClass");

            EnsureEntitiesExist(data);
            EnsureEntitiesDoNotExist(update);

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            using (var scope = GetLocalScope())
            {
                var manager = scope.Resolve<BudgetLineManager>();

                var entity = manager.AssertGetByKeyData(data.Name);

                entity.Name = update.Name;

                errors = manager.TryUpdate(entity);

                manager.SaveChanges();
            }

            // Assert ----------------------------

            errors.Should().BeEmpty();

            AssertEntitiesExist(update);
        }

        private void AssertEntitiesDoNotExist(params BudgetLineData[] data)
        {
            using (var scope = GetLocalScope())
            {
                var managerHelper = scope.Resolve<BudgetLineManagerHelper>();

                managerHelper.AssertEntitiesDoNotExist(data);
            }
        }

        private void AssertEntitiesExist(params BudgetLineData[] data)
        {
            using (var scope = GetLocalScope())
            {
                var managerHelper = scope.Resolve<BudgetLineManagerHelper>();

                managerHelper.AssertEntitiesExist(data);
            }
        }

        private void EnsureEntitiesDoNotExist(params BudgetLineData[] data)
        {
            using (var scope = GetLocalScope())
            {
                var managerHelper = scope.Resolve<BudgetLineManagerHelper>();

                managerHelper.EnsureEntitiesDoNotExist(data);
            }
        }

        private void EnsureEntitiesExist(params BudgetLineData[] data)
        {
            using (var scope = GetLocalScope())
            {
                var managerHelper = scope.Resolve<BudgetLineManagerHelper>();

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
