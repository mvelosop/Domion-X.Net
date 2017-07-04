using Autofac;
using DFlow.Tenants.Lib.Services;
using DFlow.Tenants.Lib.Tests.Helpers;
using DFlow.Tenants.Setup;
using Domion.FluentAssertions.Extensions;
using FluentAssertions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace DFlow.Tenants.Lib.Tests
{
    [Trait("Type", "Integration")]
    public class TenantManager_IntegrationTests
    {
        private static string _connectionString = "Data Source=localhost;Initial Catalog=DFlow.Tenants.Lib.Tests;Integrated Security=SSPI;MultipleActiveResultSets=true";

        private IContainer _container;

        public TenantManager_IntegrationTests()
        {
            TenantsDbSetupHelper dbHelper = SetupDatabase(_connectionString);

            _container = SetupContainer(dbHelper);
        }

        [Fact]
        public void TryDelete_DeletesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new TenantData("Delete-Success-Valid - Inserted");

            EnsureEntitiesExist(data);

            // Act -------------------------------

            IEnumerable<ValidationResult> errors;

            using (var scope = GetLocalScope())
            {
                var manager = scope.Resolve<TenantManager>();

                var entity = manager.AssertGetByKeyData(data.Owner);

                errors = manager.TryDelete(entity).ToList();

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

            var data = new TenantData("Insert-Error-Duplicate - Inserted");

            EnsureEntitiesExist(data);

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            using (var scope = GetLocalScope())
            {
                var manager = scope.Resolve<TenantManager>();

                var entity = data.CreateEntity();

                errors = manager.TryInsert(entity).ToList();
            }

            // Assert ----------------------------

            errors.Should().ContainErrorMessage(TenantManager.duplicateByOwnerError);
        }

        [Fact]
        public void TryInsert_InsertsRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new TenantData("Insert-Success-Valid - Inserted");

            EnsureEntitiesDoNotExist(data);

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            using (var scope = GetLocalScope())
            {
                var manager = scope.Resolve<TenantManager>();

                var entity = data.CreateEntity();

                errors = manager.TryInsert(entity).ToList();

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

            var dataFirst = new TenantData("Update-Error-Duplicate - Inserted first");
            var dataSecond = new TenantData("Update-Error-Duplicate - Inserted second");

            EnsureEntitiesExist(dataFirst, dataSecond);

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            using (var scope = GetLocalScope())
            {
                var manager = scope.Resolve<TenantManager>();

                var entity = manager.AssertGetByKeyData(dataFirst.Owner);

                entity.Owner = dataSecond.Owner;

                errors = manager.TryUpdate(entity).ToList();
            }

            // Assert ----------------------------

            errors.Should().ContainErrorMessage(TenantManager.duplicateByOwnerError);
        }

        [Fact]
        public void TryUpdate_UpdatesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new TenantData("Update-Success-Valid - Inserted");
            var update = new TenantData("Update-Success-Valid - Updated");

            EnsureEntitiesExist(data);
            EnsureEntitiesDoNotExist(update);

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            using (var scope = GetLocalScope())
            {
                var manager = scope.Resolve<TenantManager>();

                var entity = manager.AssertGetByKeyData(data.Owner);

                entity.Owner = update.Owner;

                errors = manager.TryUpdate(entity).ToList();

                manager.SaveChanges();
            }

            // Assert ----------------------------

            errors.Should().BeEmpty();

            AssertEntitiesExist(update);
        }

        private void AssertEntitiesDoNotExist(params TenantData[] data)
        {
            using (var scope = GetLocalScope())
            {
                var managerHelper = scope.Resolve<TenantManagerHelper>();

                managerHelper.AssertEntitiesDoNotExist(data);
            }
        }

        private void AssertEntitiesExist(params TenantData[] data)
        {
            using (var scope = GetLocalScope())
            {
                var managerHelper = scope.Resolve<TenantManagerHelper>();

                managerHelper.AssertEntitiesExist(data);
            }
        }

        private void EnsureEntitiesDoNotExist(params TenantData[] data)
        {
            using (var scope = GetLocalScope())
            {
                var managerHelper = scope.Resolve<TenantManagerHelper>();

                managerHelper.EnsureEntitiesDoNotExist(data);
            }
        }

        private void EnsureEntitiesExist(params TenantData[] data)
        {
            using (var scope = GetLocalScope())
            {
                var managerHelper = scope.Resolve<TenantManagerHelper>();

                managerHelper.EnsureEntitiesExist(data);
            }
        }

        private ILifetimeScope GetLocalScope(IContainer scope = null)
        {
            IContainer container = scope ?? _container;

            return container.BeginLifetimeScope();
        }

        private IContainer SetupContainer(TenantsDbSetupHelper dbHelper)
        {
            var autofacHelper = new TenantsAutofacSetupHelper(dbHelper);

            var builder = new ContainerBuilder();

            autofacHelper.SetupContainer(builder);

            IContainer container = builder.Build();

            return container;
        }

        private TenantsDbSetupHelper SetupDatabase(string connectionString)
        {
            TenantsDbSetupHelper dbHelper = new TenantsDbSetupHelper(_connectionString);

            dbHelper.SetupDatabase();

            return dbHelper;
        }
    }
}
