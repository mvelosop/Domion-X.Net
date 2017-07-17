using Autofac;
using DFlow.Tenants.Core.Model;
using DFlow.Tenants.Lib.Services;
using DFlow.Tenants.Lib.Tests.Helpers;
using DFlow.Tenants.Setup;
using Domion.FluentAssertions.Extensions;
using Domion.Lib.Data;
using Domion.Lib.Extensions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace DFlow.Tenants.Lib.Tests
{
    [Trait("Type", "Integration")]
    public class TenantManager_IntegrationTests
    {
        private const string ConnectionString = "Data Source=localhost;Initial Catalog=DFlow.Tenants.Lib.Tests;Integrated Security=SSPI;MultipleActiveResultSets=true";

        private static readonly IContainer Container;
        private static readonly TenantsDbHelper DbHelper;

        static TenantManager_IntegrationTests()
        {
            DbHelper = SetupDatabase(ConnectionString);

            Container = SetupContainer(DbHelper);
        }

        [Fact]
        public void TryDelete_DeletesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new TenantData("Delete-Success-Valid - Inserted");

            UsingManagerHelper((scope, helper) =>
            {
                helper.EnsureEntitiesExist(data);
            });

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            UsingManager((scope, manager) =>
            {
                Tenant entity = manager.SingleOrDefault(e => e.Owner == data.Owner);

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

            var data = new TenantData("Insert-Error-Duplicate - Inserted");

            UsingManagerHelper((scope, helper) =>
            {
                helper.EnsureEntitiesExist(data);
            });

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            UsingManager((scope, manager) =>
            {
                var mapper = scope.Resolve<TenantDataMapper>();

                Tenant entity = mapper.CreateEntity(data);

                errors = manager.TryInsert(entity).ToList();
            });

            // Assert ----------------------------

            errors.Should().ContainErrorMessage(TenantManager.DuplicateByOwnerError);
        }

        [Fact]
        public void TryInsert_InsertsRecord_WhenValidData()
        {
            IEnumerable<ValidationResult> errors = null;

            // Arrange ---------------------------

            var data = new TenantData("Insert-Success-Valid - Inserted");

            UsingManagerHelper((scope, helper) =>
            {
                helper.EnsureEntitiesDoNotExist(data);
            });

            // Act -------------------------------

            UsingManager((scope, manager) =>
            {
                var mapper = scope.Resolve<TenantDataMapper>();

                Tenant entity = mapper.CreateEntity(data);

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
        public void TryDelete_Fails_WhenConcurrencyConflict()
        {
            IEnumerable<ValidationResult> errors = null;

            // Arrange ---------------------------

            var data = new TenantData("Delete-Error-Concurrency - Inserted");
            var update = new TenantData("Delete-Error-Concurrency - Updated");

            UsingManagerHelper((scope, helper) =>
            {
                helper.EnsureEntitiesExist(data);
                helper.EnsureEntitiesDoNotExist(update);
            });

            // To simulate a ViewModel and second user updating the record

            var viewModel = new Tenant();

            UsingManager((scope, manager) =>
            {
                var mapper = scope.Resolve<TenantDataMapper>();

                Tenant entity = manager.SingleOrDefault(e => e.Owner == data.Owner);

                viewModel = mapper.DuplicateEntity(entity);

                mapper.UpdateEntity(entity, update);

                errors = manager.TryUpdate(entity).ToList();

                errors.Should().BeEmpty();

                manager.SaveChanges();
            });

            // Act -------------------------------

            UsingManager((scope, manager) =>
            {
                Tenant entity = manager.SingleOrDefault(e => e.Id == viewModel.Id);

                entity.RowVersion = viewModel.RowVersion;

                errors = manager.TryDelete(entity).ToList();

                manager.SaveChanges();
            });

            // Assert ----------------------------

            errors.Should().ContainErrorMessage(TenantManager.ConcurrentUpdateError);

            UsingManagerHelper((scope, helper) =>
            {
                helper.AssertEntitiesDoNotExist(data);
                helper.AssertEntitiesExist(update);
            });
        }

        [Fact]
        public void TryUpdate_Fails_WhenConcurrencyConflict()
        {
            IEnumerable<ValidationResult> errors = null;

            // Arrange ---------------------------

            var data = new TenantData("Update-Error-Concurrency - Inserted");
            var update1 = new TenantData("Update-Error-Concurrency - Updated 1");
            var update2 = new TenantData("Update-Error-Concurrency - Updated 2");

            UsingManagerHelper((scope, helper) =>
            {
                helper.EnsureEntitiesExist(data);
                helper.EnsureEntitiesDoNotExist(update1, update2);
            });

            // To simulate a ViewModel and second user updating the record

            var viewModel = new Tenant();

            UsingManager((scope, manager) =>
            {
                var mapper = scope.Resolve<TenantDataMapper>();

                Tenant entity = manager.SingleOrDefault(e => e.Owner == data.Owner);

                viewModel = mapper.DuplicateEntity(entity);

                mapper.UpdateEntity(entity, update2);

                errors = manager.TryUpdate(entity).ToList();

                errors.Should().BeEmpty();

                manager.SaveChanges();
            });

            // Act -------------------------------

            UsingManager((scope, manager) =>
            {
                var mapper = scope.Resolve<TenantDataMapper>();

                Tenant entity = manager.SingleOrDefault(e => e.Id == viewModel.Id);

                entity = mapper.UpdateEntity(entity, update1);

                entity.RowVersion = viewModel.RowVersion;

                errors = manager.TryUpdate(entity).ToList();
            });

            // Assert ----------------------------

            errors.Should().ContainErrorMessage(TenantManager.ConcurrentUpdateError);

            UsingManagerHelper((scope, helper) =>
            {
                helper.AssertEntitiesDoNotExist(data, update1);
                helper.AssertEntitiesExist(update2);
            });
        }

        [Fact]
        public void TryUpdate_Fails_WhenDuplicateKeyData()
        {
            // Arrange ---------------------------

            var data = new TenantData("Update-Error-Duplicate - Inserted first");
            var update = new TenantData("Update-Error-Duplicate - Inserted second");

            UsingManagerHelper((scope, helper) =>
            {
                helper.EnsureEntitiesExist(data, update);
            });

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            UsingManager((scope, manager) =>
            {
                var mapper = scope.Resolve<TenantDataMapper>();

                Tenant entity = manager.SingleOrDefault(e => e.Owner == data.Owner);

                entity = mapper.UpdateEntity(entity, update);

                errors = manager.TryUpdate(entity).ToList();
            });

            // Assert ----------------------------

            errors.Should().ContainErrorMessage(TenantManager.DuplicateByOwnerError);
        }

        [Fact]
        public void TryUpdate_UpdatesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new TenantData("Update-Success-Valid - Inserted");
            var update = new TenantData("Update-Success-Valid - Updated");

            UsingManagerHelper((scope, helper) =>
            {
                helper.EnsureEntitiesExist(data);
                helper.EnsureEntitiesDoNotExist(update);
            });

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            UsingManager((scope, manager) =>
            {
                var mapper = scope.Resolve<TenantDataMapper>();

                Tenant entity = manager.SingleOrDefault(e => e.Owner == data.Owner);

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

        private static IContainer SetupContainer(TenantsDbHelper dbHelper)
        {
            var containerSetup = new TenantsContainerSetup(dbHelper);

            var builder = new ContainerBuilder();

            containerSetup.RegisterTypes(builder);

            IContainer container = builder.Build();

            return container;
        }

        private static TenantsDbHelper SetupDatabase(string connectionString)
        {
            var dbHelper = new TenantsDbHelper(connectionString);

            dbHelper.SetupDatabase();

            return dbHelper;
        }

        private void UpdateOnDifferentDbContext(TenantData data, TenantData update)
        {
            IEnumerable<ValidationResult> errors = null;

            UsingManager((scope, manager) =>
            {
                var mapper = scope.Resolve<TenantDataMapper>();

                Tenant entity = manager.SingleOrDefault(e => e.Owner == data.Owner);

                entity = mapper.UpdateEntity(entity, update);

                errors = manager.TryUpdate(entity).ToList();

                manager.SaveChanges();
            });

            errors.Should().BeEmpty();
        }

        private void UsingManager(Action<ILifetimeScope, TenantManager> action)
        {
            using (ILifetimeScope scope = Container.BeginLifetimeScope())
            {
                var manager = scope.Resolve<TenantManager>();

                action.Invoke(scope, manager);
            }
        }

        private void UsingManagerHelper(Action<ILifetimeScope, TenantManagerHelper> action)
        {
            using (ILifetimeScope scope = Container.BeginLifetimeScope())
            {
                var helper = scope.Resolve<TenantManagerHelper>();

                action.Invoke(scope, helper);
            }
        }
    }
}
