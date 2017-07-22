using Autofac;
using DFlow.Budget.Core.Model;
using DFlow.Budget.Lib.Services;
using DFlow.Budget.Lib.Tests.Helpers;
using DFlow.Budget.Setup;
using DFlow.Tenants.Core.Model;
using DFlow.Tenants.Lib.Services;
using DFlow.Tenants.Lib.Tests.Extensions;
using DFlow.Tenants.Lib.Tests.Helpers;
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
    public class BudgetClassRepository_IntegrationTests
    {
        private const string ConnectionString = "Data Source=localhost;Initial Catalog=DFlow.Budget.Lib.Tests;Integrated Security=SSPI;MultipleActiveResultSets=true";

        private static readonly IContainer Container;
        private static readonly BudgetDbHelper DbHelper;
        private static readonly TenantData DefaultTenantData = new TenantData("Default tenant");
        private static readonly TenantData TenantAData = new TenantData("Tenant A");
        private static readonly TenantData TenantBData = new TenantData("Tenant B");

        private readonly Tenant DefaultTenant;
        private readonly Tenant TenantA;
        private readonly Tenant TenantB;

        static BudgetClassRepository_IntegrationTests()
        {
            DbHelper = SetupDatabase(ConnectionString);

            Container = SetupContainer(DbHelper);

            SeedBaseData(DbHelper);
        }

        public BudgetClassRepository_IntegrationTests()
        {
            using (ILifetimeScope scope = Container.BeginLifetimeScope())
            {
                var repo = scope.Resolve<TenantRepository>();

                DefaultTenant = repo.AssertGetByKeyData(DefaultTenantData.Owner);
                TenantA = repo.AssertGetByKeyData(TenantAData.Owner);
                TenantB = repo.AssertGetByKeyData(TenantBData.Owner);
            }
        }

        [Fact]
        public void TryDelete_DeletesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new BudgetClassData("Delete-Success-Valid - Inserted", TransactionType.Income);

            UsingManagerHelper((scope, helper) =>
            {
                helper.EnsureEntitiesExist(data);
            });

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            UsingManager((scope, manager) =>
            {
                BudgetClass entity = manager.SingleOrDefault(bc => bc.Name == data.Name);

                errors = manager.TryDelete(entity).ToList();

                manager.SaveChanges();
            });

            // Assert ----------------------------

            errors.Should().BeEmpty();

            UsingManagerHelper((scope, helper) =>
            {
                helper.AssertEntitiesDoNotExist(data);
            });
        }

        [Fact]
        public void TryInsert_Fails_WhenDuplicateKeyData()
        {
            // Arrange ---------------------------

            var data = new BudgetClassData("Insert-Error-Duplicate - Inserted", TransactionType.Income);

            UsingManagerHelper((scope, helper) =>
            {
                helper.EnsureEntitiesExist(data);
            });

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            UsingManager((scope, manager) =>
            {
                var mapper = scope.Resolve<BudgetClassDataMapper>();

                BudgetClass entity = mapper.CreateEntity(data);

                errors = manager.TryInsert(entity).ToList();
            });

            // Assert ----------------------------

            errors.Should().ContainErrorMessage(BudgetClassRepository.duplicateByNameError);
        }

        [Fact]
        public void TryInsert_InsertsRecord_WhenValidData()
        {
            IEnumerable<ValidationResult> errors = null;

            // Arrange ---------------------------

            var data = new BudgetClassData("Insert-Success-Valid - Inserted", TransactionType.Income);

            UsingManagerHelper((scope, helper) =>
            {
                helper.EnsureEntitiesDoNotExist(data);
            });

            // Act -------------------------------

            UsingManager((scope, manager) =>
            {
                var mapper = scope.Resolve<BudgetClassDataMapper>();

                BudgetClass entity = mapper.CreateEntity(data);

                errors = manager.TryInsert(entity).ToList();

                manager.SaveChanges();
            });

            // Assert ----------------------------

            errors.Should().BeEmpty();

            UsingManagerHelper((scope, helper) =>
            {
                helper.AssertEntitiesExist(data);
            });
        }

        [Fact]
        public void TryInsert_InsertsRecords_WhenSameNameOnDifferentTenants()
        {
            IEnumerable<ValidationResult> errorsA = null;
            IEnumerable<ValidationResult> errorsB = null;

            // Arrange ---------------------------

            var data = new BudgetClassData("Insert-Success-Valid - Inserted", TransactionType.Income);

            UsingManagerHelper(TenantA, (scope, helper) =>
            {
                helper.EnsureEntitiesDoNotExist(data);
            });

            UsingManagerHelper(TenantB, (scope, helper) =>
            {
                helper.EnsureEntitiesDoNotExist(data);
            });

            // Act -------------------------------

            UsingManager(TenantA, (scope, manager) =>
            {
                var mapper = scope.Resolve<BudgetClassDataMapper>();

                BudgetClass entity = mapper.CreateEntity(data);

                errorsA = manager.TryInsert(entity).ToList();

                manager.SaveChanges();
            });

            UsingManager(TenantB, (scope, manager) =>
            {
                var mapper = scope.Resolve<BudgetClassDataMapper>();

                BudgetClass entity = mapper.CreateEntity(data);

                errorsB = manager.TryInsert(entity).ToList();

                manager.SaveChanges();
            });

            // Assert ----------------------------

            errorsA.Should().BeEmpty();
            errorsB.Should().BeEmpty();

            UsingManagerHelper(TenantA, (scope, helper) =>
            {
                helper.AssertEntitiesExist(data);
            });

            UsingManagerHelper(TenantA, (scope, helper) =>
            {
                helper.AssertEntitiesExist(data);
            });
        }

        [Fact]
        public void TryUpdate_Fails_WhenDuplicateKeyData()
        {
            // Arrange ---------------------------

            var data = new BudgetClassData("Update-Error-Duplicate - Inserted first", TransactionType.Income);
            var update = new BudgetClassData("Update-Error-Duplicate - Inserted second", TransactionType.Income);

            UsingManagerHelper((scope, helper) =>
            {
                helper.EnsureEntitiesExist(data, update);
            });

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            UsingManager((scope, manager) =>
            {
                var mapper = scope.Resolve<BudgetClassDataMapper>();

                BudgetClass entity = manager.SingleOrDefault(bc => bc.Name == data.Name);

                entity = mapper.UpdateEntity(entity, update);

                errors = manager.TryUpdate(entity).ToList();
            });

            // Assert ----------------------------

            errors.Should().ContainErrorMessage(BudgetClassRepository.duplicateByNameError);
        }

        [Fact]
        public void TryUpdate_UpdatesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new BudgetClassData("Update-Success-Valid - Inserted", TransactionType.Income);
            var update = new BudgetClassData("Update-Success-Valid - Updated", TransactionType.Income);

            UsingManagerHelper((scope, helper) =>
            {
                helper.EnsureEntitiesExist(data);
                helper.EnsureEntitiesDoNotExist(update);
            });

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            UsingManager((scope, manager) =>
            {
                var mapper = scope.Resolve<BudgetClassDataMapper>();

                BudgetClass entity = manager.SingleOrDefault(bc => bc.Name == data.Name);

                entity = mapper.UpdateEntity(entity, update);

                errors = manager.TryUpdate(entity).ToList();

                manager.SaveChanges();
            });

            // Assert ----------------------------

            errors.Should().BeEmpty();

            UsingManagerHelper((scope, helper) =>
            {
                helper.AssertEntitiesExist(update);
            });
        }

        private static void SeedBaseData(BudgetDbHelper budgetDbHelper)
        {
            using (ILifetimeScope scope = Container.BeginLifetimeScope())
            {
                var helper = scope.Resolve<TenantManagerHelper>();

                helper.EnsureEntitiesExist(DefaultTenantData, TenantAData, TenantBData);
            }
        }

        private static IContainer SetupContainer(BudgetDbHelper dbHelper)
        {
            var containerSetup = new BudgetContainerSetup(dbHelper);

            var builder = new ContainerBuilder();

            containerSetup.RegisterTypes(builder);

            IContainer container = builder.Build();

            return container;
        }

        private static BudgetDbHelper SetupDatabase(string connectionString)
        {
            var dbHelper = new BudgetDbHelper(connectionString);

            dbHelper.SetupDatabase();

            return dbHelper;
        }

        private Action<ILifetimeScope, BudgetClassRepository> GetTenant(TenantData tenantData)
        {
            throw new NotImplementedException();
        }

        private void UsingManager(Action<ILifetimeScope, BudgetClassRepository> action)
        {
            UsingManager(DefaultTenant, action);
        }

        private void UsingManager(Tenant currenTenant, Action<ILifetimeScope, BudgetClassRepository> action)
        {
            using (ILifetimeScope scope = Container.BeginLifetimeScope(cb => cb.Register(c => currenTenant)))
            {
                var repo = scope.Resolve<BudgetClassRepository>();

                action.Invoke(scope, repo);
            }
        }

        private void UsingManagerHelper(Action<ILifetimeScope, BudgetClassManagerHelper> action)
        {
            UsingManagerHelper(DefaultTenant, action);
        }

        private void UsingManagerHelper(Tenant currenTenant, Action<ILifetimeScope, BudgetClassManagerHelper> action)
        {
            using (ILifetimeScope scope = Container.BeginLifetimeScope(cb => cb.Register(c => currenTenant)))
            {
                var helper = scope.Resolve<BudgetClassManagerHelper>();

                action.Invoke(scope, helper);
            }
        }
    }
}
