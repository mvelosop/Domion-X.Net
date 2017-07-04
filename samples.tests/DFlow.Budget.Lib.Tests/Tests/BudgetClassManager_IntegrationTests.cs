using Autofac;
using DFlow.Budget.Core.Model;
using DFlow.Budget.Lib.Services;
using DFlow.Budget.Lib.Tests.Helpers;
using DFlow.Budget.Setup;
using DFlow.Tenants.Core.Model;
using DFlow.Tenants.Lib.Services;
using DFlow.Tenants.Lib.Tests.Helpers;
using DFlow.Tenants.Setup;
using Domion.FluentAssertions.Extensions;
using Domion.Lib.Extensions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace DFlow.Budget.Lib.Tests.Tests
{
    [Trait("Type", "Integration")]
    public class BudgetClassManager_IntegrationTests
    {
        private const string ConnectionString = "Data Source=localhost;Initial Catalog=DFlow.Budget.Lib.Tests;Integrated Security=SSPI;MultipleActiveResultSets=true";

        private static readonly BudgetDbSetupHelper DbSetupHelper;

        private readonly IContainer Container;

        static BudgetClassManager_IntegrationTests()
        {
            DbSetupHelper = SetupDatabase(ConnectionString);
        }

        public BudgetClassManager_IntegrationTests()
        {
            Container = SetupContainer(DbSetupHelper);
        }

        [Fact]
        public void TryDelete_DeletesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            const string owner = "Reference Tenant #1";

            var data = new BudgetClassData("Delete-Success-Valid - Inserted", TransactionType.Income);

            UsingManagerHelper(owner, (scope, helper) =>
            {
                helper.EnsureEntitiesExist(data);
            });

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            UsingManager(owner, (scope, manager) =>
            {
                BudgetClass entity = manager.SingleOrDefault(bc => bc.Name == data.Name);

                errors = manager.TryDelete(entity).ToList();

                manager.SaveChanges();
            });

            // Assert ----------------------------

            errors.Should().BeEmpty();

            UsingManagerHelper(owner, (scope, helper) =>
            {
                helper.AssertEntitiesDoNotExist(data);
            });
        }

        [Fact]
        public void TryInsert_Fails_WhenDuplicateKeyData()
        {
            // Arrange ---------------------------

            const string owner = "Reference Tenant #1";

            var data = new BudgetClassData("Insert-Error-Duplicate - Inserted", TransactionType.Income);

            UsingManagerHelper(owner, (scope, helper) =>
            {
                helper.EnsureEntitiesExist(data);
            });

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            UsingManager(owner, (scope, manager) =>
            {
                var mapper = scope.Resolve<BudgetClassDataMapper>();

                BudgetClass entity = mapper.CreateEntity(data);

                errors = manager.TryInsert(entity).ToList();
            });

            // Assert ----------------------------

            errors.Should().ContainErrorMessage(BudgetClassManager.duplicateByNameError);
        }

        [Fact]
        public void TryInsert_InsertsRecord_WhenValidData()
        {
            IEnumerable<ValidationResult> errors = null;

            // Arrange ---------------------------

            const string owner = "Reference Tenant #1";

            var data = new BudgetClassData("Insert-Success-Valid - Inserted", TransactionType.Income);

            UsingManagerHelper(owner, (scope, helper) =>
            {
                helper.EnsureEntitiesDoNotExist(data);
            });

            // Act -------------------------------

            UsingManager(owner, (scope, manager) =>
            {
                var mapper = scope.Resolve<BudgetClassDataMapper>();

                BudgetClass entity = mapper.CreateEntity(data);

                errors = manager.TryInsert(entity).ToList();

                manager.SaveChanges();
            });

            // Assert ----------------------------

            errors.Should().BeEmpty();

            UsingManagerHelper(owner, (scope, helper) =>
            {
                helper.AssertEntitiesExist(data);
            });
        }

        [Fact]
        public void TryInsert_InsertsRecords_WhenDuplicateKeyDataButDifferentTenants()
        {
            // Arrange ---------------------------

            const string firstOwner = "Reference Tenant #1";
            const string secondOwner = "Reference Tenant #2";

            var data = new BudgetClassData("Insert-Success-Duplicate-Different-Tenant - Inserted", TransactionType.Income);

            UsingManagerHelper(firstOwner, (scope, helper) =>
            {
                helper.EnsureEntitiesDoNotExist(data);
            });

            UsingManagerHelper(secondOwner, (scope, helper) =>
            {
                helper.EnsureEntitiesDoNotExist(data);
            });

            // Act -------------------------------

            IEnumerable<ValidationResult> errorsFirst = null;
            IEnumerable<ValidationResult> errorsSecond = null;

            UsingManager(firstOwner, (scope, manager) =>
            {
                var mapper = scope.Resolve<BudgetClassDataMapper>();

                BudgetClass entity = mapper.CreateEntity(data);

                errorsFirst = manager.TryInsert(entity).ToList();

                manager.SaveChanges();
            });

            UsingManager(secondOwner, (scope, manager) =>
            {
                var mapper = scope.Resolve<BudgetClassDataMapper>();

                BudgetClass entity = mapper.CreateEntity(data);

                errorsSecond = manager.TryInsert(entity).ToList();

                manager.SaveChanges();
            });

            // Assert ----------------------------

            errorsFirst.Should().BeEmpty();
            errorsSecond.Should().BeEmpty();
        }

        [Fact]
        public void TryUpdate_Fails_WhenDuplicateKeyData()
        {
            // Arrange ---------------------------

            const string owner = "Reference Tenant #1";

            var dataFirst = new BudgetClassData("Update-Error-Duplicate - Inserted first", TransactionType.Income);
            var dataSecond = new BudgetClassData("Update-Error-Duplicate - Inserted second", TransactionType.Income);

            UsingManagerHelper(owner, (scope, helper) =>
            {
                helper.EnsureEntitiesExist(dataFirst, dataSecond);
            });

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            UsingManager(owner, (scope, manager) =>
            {
                BudgetClass entity = manager.SingleOrDefault(bc => bc.Name == dataFirst.Name);

                entity.Name = dataSecond.Name;

                errors = manager.TryUpdate(entity).ToList();
            });

            // Assert ----------------------------

            errors.Should().ContainErrorMessage(BudgetClassManager.duplicateByNameError);
        }

        [Fact]
        public void TryUpdate_UpdatesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            const string owner = "Reference Tenant #1";

            var data = new BudgetClassData("Update-Success-Valid - Inserted", TransactionType.Income);
            var update = new BudgetClassData("Update-Success-Valid - Updated", TransactionType.Income);

            UsingManagerHelper(owner, (scope, helper) =>
            {
                helper.EnsureEntitiesExist(data);
                helper.EnsureEntitiesDoNotExist(update);
            });

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            UsingManager(owner, (scope, manager) =>
            {
                BudgetClass entity = manager.AssertGetByKeyData(data.Name);

                entity.Name = update.Name;

                errors = manager.TryUpdate(entity).ToList();

                manager.SaveChanges();
            });

            // Assert ----------------------------

            errors.Should().BeEmpty();

            UsingManagerHelper(owner, (scope, helper) =>
            {
                helper.AssertEntitiesExist(update);
            });
        }

        private static IContainer SetupContainer(BudgetDbSetupHelper dbHelper)
        {
            var containerSetup = new BudgetContainerSetup(dbHelper);

            var builder = new ContainerBuilder();

            containerSetup.RegisterTypes(builder);

            // Tenants ---------------------------

            var TenantsDbHelper = new TenantsDbSetupHelper(ConnectionString);

            TenantsDbHelper.SetupDatabase();

            var TenantsDiHelper = new TenantsAutofacSetupHelper(TenantsDbHelper);

            TenantsDiHelper.SetupContainer(builder);

            //------------------------------------

            IContainer container = builder.Build();

            return container;
        }

        private static BudgetDbSetupHelper SetupDatabase(string connectionString)
        {
            var dbHelper = new BudgetDbSetupHelper(connectionString);

            dbHelper.SetupDatabase();

            return dbHelper;
        }

        private Tenant GetTenant(string owner)
        {
            Tenant tenant;

            using (ILifetimeScope scope = Container.BeginLifetimeScope())
            {
                var manager = scope.Resolve<TenantManager>();

                tenant = manager.SingleOrDefault(t => t.Owner == owner);
            }

            return tenant;
        }

        private void SetupReferenceData()
        {
            using (ILifetimeScope scope = Container.BeginLifetimeScope())
            {
                var TenantsDataHelper = new TenantsTestDataHelper(scope);

                TenantsDataHelper.SeedReferenceTenants();
            }
        }

        private void UsingManager(string owner, Action<ILifetimeScope, BudgetClassManager> action)
        {
            Tenant tenant = GetTenant(owner);

            using (ILifetimeScope scope = Container.BeginLifetimeScope())
            {
                var manager = scope.Resolve<BudgetClassManager>();

                action.Invoke(scope, manager);
            }
        }

        private void UsingManagerHelper(string owner, Action<ILifetimeScope, BudgetClassManagerHelper> action)
        {
            using (ILifetimeScope scope = Container.BeginLifetimeScope())
            {
                var helper = scope.Resolve<BudgetClassManagerHelper>();

                action.Invoke(scope, helper);
            }
        }
    }
}
