using Autofac;
using DFlow.Budget.Lib.Services;
using DFlow.Budget.Lib.Tests.Helpers;
using DFlow.Budget.Setup;
using DFlow.Tennants.Core.Model;
using DFlow.Tennants.Lib.Services;
using DFlow.Tennants.Lib.Tests.Helpers;
using DFlow.Tennants.Setup;
using Domion.FluentAssertions.Extensions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

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

            SetupReferenceData();
        }

        [Fact]
        public void TryDelete_DeletesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new BudgetClassData("Delete-Success-Valid - Inserted", "Income");

            var tennantContext = GetTennantContext("Reference Tennant #1");

            EnsureEntitiesExist(tennantContext, data);

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            using (var scope = GetLocalScope(tennantContext))
            {
                var manager = scope.Resolve<BudgetClassManager>();

                var entity = manager.AssertGetByKeyData(data.Name);

                errors = manager.TryDelete(entity).ToList();

                manager.SaveChanges();
            }

            // Assert ----------------------------

            errors.Should().BeEmpty();

            AssertEntitiesDoNotExist(tennantContext, data);
        }

        [Fact]
        public void TryInsert_Fails_WhenDuplicateKeyData()
        {
            // Arrange ---------------------------

            var data = new BudgetClassData("Insert-Error-Duplicate - Inserted", "Income");

            var tennantContext = GetTennantContext("Reference Tennant #1");

            EnsureEntitiesExist(tennantContext, data);

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            using (var scope = GetLocalScope(tennantContext))
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

            var data = new BudgetClassData("Insert-Success-Valid - Inserted", "Income");

            var tennantContext = GetTennantContext("Reference Tennant #1");

            EnsureEntitiesDoNotExist(tennantContext, data);

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            using (var scope = GetLocalScope(tennantContext))
            {
                var manager = scope.Resolve<BudgetClassManager>();

                var entity = data.CreateEntity();

                errors = manager.TryInsert(entity).ToList();

                manager.SaveChanges();
            }

            // Assert ----------------------------

            errors.Should().BeEmpty();

            AssertEntitiesExist(tennantContext, data);
        }

        [Fact]
        public void TryUpdate_Fails_WhenDuplicateKeyData()
        {
            // Arrange ---------------------------

            var dataFirst = new BudgetClassData("Update-Error-Duplicate - Inserted first", "Income");
            var dataSecond = new BudgetClassData("Update-Error-Duplicate - Inserted second", "Income");

            var tennantContext = GetTennantContext("Reference Tennant #1");

            EnsureEntitiesExist(tennantContext, dataFirst, dataSecond);

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            using (var scope = GetLocalScope(tennantContext))
            {
                var manager = scope.Resolve<BudgetClassManager>();

                var entity = manager.AssertGetByKeyData(dataFirst.Name);

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

            var data = new BudgetClassData("Update-Success-Valid - Inserted", "Income");
            var update = new BudgetClassData("Update-Success-Valid - Updated", "Income");

            var tennantContext = GetTennantContext("Reference Tennant #1");

            EnsureEntitiesExist(tennantContext, data);
            EnsureEntitiesDoNotExist(tennantContext, update);

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            using (var scope = GetLocalScope(tennantContext))
            {
                var manager = scope.Resolve<BudgetClassManager>();

                var entity = manager.AssertGetByKeyData(data.Name);

                entity.Name = update.Name;

                errors = manager.TryUpdate(entity).ToList();

                manager.SaveChanges();
            }

            // Assert ----------------------------

            errors.Should().BeEmpty();

            AssertEntitiesExist(tennantContext, update);
        }

        private void AssertEntitiesDoNotExist(Action<ContainerBuilder> scopeContext, params BudgetClassData[] data)
        {
            using (var scope = GetLocalScope(scopeContext))
            {
                var managerHelper = scope.Resolve<BudgetClassManagerHelper>();

                managerHelper.AssertEntitiesDoNotExist(data);
            }
        }

        private void AssertEntitiesExist(Action<ContainerBuilder> scopeContext, params BudgetClassData[] data)
        {
            using (var scope = GetLocalScope(scopeContext))
            {
                var managerHelper = scope.Resolve<BudgetClassManagerHelper>();

                managerHelper.AssertEntitiesExist(data);
            }
        }

        private void EnsureEntitiesDoNotExist(Action<ContainerBuilder> scopeContext, params BudgetClassData[] data)
        {
            using (var scope = GetLocalScope(scopeContext))
            {
                var managerHelper = scope.Resolve<BudgetClassManagerHelper>();

                managerHelper.EnsureEntitiesDoNotExist(data);
            }
        }

        private void EnsureEntitiesExist(Action<ContainerBuilder> scopeContext, params BudgetClassData[] data)
        {
            using (var scope = GetLocalScope(scopeContext))
            {
                var managerHelper = scope.Resolve<BudgetClassManagerHelper>();

                managerHelper.EnsureEntitiesExist(data);
            }
        }

        private ILifetimeScope GetLocalScope(Action<ContainerBuilder> scopeContext, IContainer scope = null)
        {
            IContainer container = scope ?? _container;

            if (scopeContext == null)
            {
                return container.BeginLifetimeScope();
            }
            else
            {
                return container.BeginLifetimeScope(scopeContext);
            }
        }

        private Action<ContainerBuilder> GetTennantContext(string owner)
        {
            Tennant tennant = null;

            using (var scope = GetLocalScope(null))
            {
                var manager = scope.Resolve<TennantManager>();

                tennant = manager.AssertGetByKeyData(owner);
            }

            Action<ContainerBuilder> scopeContext = cb =>
            {
                cb.Register((c) => tennant);
            };

            return scopeContext;
        }

        private IContainer SetupContainer(BudgetDbSetupHelper dbHelper)
        {
            var budgetDiHelper = new BudgetAutofacSetupHelper(dbHelper);

            var builder = new ContainerBuilder();

            budgetDiHelper.SetupContainer(builder);

            // Tennants --------------------------

            TennantsDbSetupHelper tennantsDbHelper = new TennantsDbSetupHelper(_connectionString);

            tennantsDbHelper.SetupDatabase();

            var tennantsDiHelper = new TennantsAutofacSetupHelper(tennantsDbHelper);

            tennantsDiHelper.SetupContainer(builder);

            //------------------------------------

            IContainer container = builder.Build();

            return container;
        }

        private BudgetDbSetupHelper SetupDatabase(string connectionString)
        {
            BudgetDbSetupHelper dbHelper = new BudgetDbSetupHelper(_connectionString);

            dbHelper.SetupDatabase();

            return dbHelper;
        }

        private void SetupReferenceData()
        {
            var tennantsDataHelper = new TennantsTestDataHelper(_container);

            tennantsDataHelper.SeedReferenceTennants();
        }
    }
}
