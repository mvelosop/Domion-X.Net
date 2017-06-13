using Autofac;
using DFlow.Tennants.Lib.Services;
using DFlow.Tennants.Lib.Tests.Helpers;
using DFlow.Tennants.Setup;
using Domion.FluentAssertions.Extensions;
using FluentAssertions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace DFlow.Tennants.Lib.Tests
{
    [Trait("Type", "Integration")]
    public class TennantManager_IntegrationTests
    {
        private static string _connectionString = "Data Source=localhost;Initial Catalog=DFlow.Tennants.Lib.Tests;Integrated Security=SSPI;MultipleActiveResultSets=true";

        private IContainer _container;

        public TennantManager_IntegrationTests()
        {
            TennantsDbSetupHelper dbHelper = SetupDatabase(_connectionString);

            _container = SetupContainer(dbHelper);
        }

        [Fact]
        public void TryDelete_DeletesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new TennantData("Delete-Success-Valid - Inserted");

            EnsureEntitiesExist(data);

            // Act -------------------------------

            IEnumerable<ValidationResult> errors;

            using (var scope = GetLocalScope())
            {
                var manager = scope.Resolve<TennantManager>();

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

            var data = new TennantData("Insert-Error-Duplicate - Inserted");

            EnsureEntitiesExist(data);

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            using (var scope = GetLocalScope())
            {
                var manager = scope.Resolve<TennantManager>();

                var entity = data.CreateEntity();

                errors = manager.TryInsert(entity).ToList();
            }

            // Assert ----------------------------

            errors.Should().ContainErrorMessage(TennantManager.duplicateByOwnerError);
        }

        [Fact]
        public void TryInsert_InsertsRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new TennantData("Insert-Success-Valid - Inserted");

            EnsureEntitiesDoNotExist(data);

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            using (var scope = GetLocalScope())
            {
                var manager = scope.Resolve<TennantManager>();

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

            var dataFirst = new TennantData("Update-Error-Duplicate - Inserted first");
            var dataSecond = new TennantData("Update-Error-Duplicate - Inserted second");

            EnsureEntitiesExist(dataFirst, dataSecond);

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            using (var scope = GetLocalScope())
            {
                var manager = scope.Resolve<TennantManager>();

                var entity = manager.AssertGetByKeyData(dataFirst.Owner);

                entity.Owner = dataSecond.Owner;

                errors = manager.TryUpdate(entity).ToList();
            }

            // Assert ----------------------------

            errors.Should().ContainErrorMessage(TennantManager.duplicateByOwnerError);
        }

        [Fact]
        public void TryUpdate_UpdatesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new TennantData("Update-Success-Valid - Inserted");
            var update = new TennantData("Update-Success-Valid - Updated");

            EnsureEntitiesExist(data);
            EnsureEntitiesDoNotExist(update);

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            using (var scope = GetLocalScope())
            {
                var manager = scope.Resolve<TennantManager>();

                var entity = manager.AssertGetByKeyData(data.Owner);

                entity.Owner = update.Owner;

                errors = manager.TryUpdate(entity).ToList();

                manager.SaveChanges();
            }

            // Assert ----------------------------

            errors.Should().BeEmpty();

            AssertEntitiesExist(update);
        }

        private void AssertEntitiesDoNotExist(params TennantData[] data)
        {
            using (var scope = GetLocalScope())
            {
                var managerHelper = scope.Resolve<TennantManagerHelper>();

                managerHelper.AssertEntitiesDoNotExist(data);
            }
        }

        private void AssertEntitiesExist(params TennantData[] data)
        {
            using (var scope = GetLocalScope())
            {
                var managerHelper = scope.Resolve<TennantManagerHelper>();

                managerHelper.AssertEntitiesExist(data);
            }
        }

        private void EnsureEntitiesDoNotExist(params TennantData[] data)
        {
            using (var scope = GetLocalScope())
            {
                var managerHelper = scope.Resolve<TennantManagerHelper>();

                managerHelper.EnsureEntitiesDoNotExist(data);
            }
        }

        private void EnsureEntitiesExist(params TennantData[] data)
        {
            using (var scope = GetLocalScope())
            {
                var managerHelper = scope.Resolve<TennantManagerHelper>();

                managerHelper.EnsureEntitiesExist(data);
            }
        }

        private ILifetimeScope GetLocalScope(IContainer scope = null)
        {
            IContainer container = scope ?? _container;

            return container.BeginLifetimeScope();
        }

        private IContainer SetupContainer(TennantsDbSetupHelper dbHelper)
        {
            var autofacHelper = new TennantsAutofacSetupHelper(dbHelper);

            var builder = new ContainerBuilder();

            autofacHelper.SetupContainer(builder);

            IContainer container = builder.Build();

            return container;
        }

        private TennantsDbSetupHelper SetupDatabase(string connectionString)
        {
            TennantsDbSetupHelper dbHelper = new TennantsDbSetupHelper(_connectionString);

            dbHelper.SetupDatabase();

            return dbHelper;
        }
    }
}
