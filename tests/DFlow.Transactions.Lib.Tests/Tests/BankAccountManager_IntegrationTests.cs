using Autofac;
using DFlow.Transactions.Lib.Services;
using DFlow.Transactions.Lib.Tests.Helpers;
using DFlow.Transactions.Setup;
using Domion.FluentAssertions.Extensions;
using FluentAssertions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace DFlow.Transactions.Lib.Tests.Tests
{
    [Trait("Type", "Integration")]
    public class BankAccountManager_IntegrationTests
    {
        private static string _connectionString = "Data Source=localhost;Initial Catalog=DFlow.Transactions.Lib.Tests;Integrated Security=SSPI;MultipleActiveResultSets=true";

        private IContainer _container;

        public BankAccountManager_IntegrationTests()
        {
            TransactionsDbSetupHelper dbHelper = SetupDatabase(_connectionString);

            _container = SetupContainer(dbHelper);
        }

        [Fact]
        public void TryDelete_DeletesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new BankAccountData("Delete-Success-Valid - Inserted", "003-CRUD", "BANK-3");

            EnsureEntitiesExist(data);

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            using (var scope = GetLocalScope())
            {
                var manager = scope.Resolve<BankAccountManager>();

                var entity = manager.AssertGetByKeyData(data.AccountName);

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

            var data = new BankAccountData("Insert-Error-Duplicate - Inserted", "004-CRUD", "BANK-4");

            EnsureEntitiesExist(data);

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            using (var scope = GetLocalScope())
            {
                var manager = scope.Resolve<BankAccountManager>();

                var entity = data.CreateEntity();

                errors = manager.TryInsert(entity).ToList();
            }

            // Assert ----------------------------

            errors.Should().ContainErrorMessage(BankAccountManager.duplicateByAccountNameError);
        }

        [Fact]
        public void TryInsert_InsertsRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new BankAccountData("Insert-Success-Valid - Inserted", "001-CRUD", "BANK-1");

            EnsureEntitiesDoNotExist(data);

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            using (var scope = GetLocalScope())
            {
                var manager = scope.Resolve<BankAccountManager>();

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

            var dataFirst = new BankAccountData("Update-Error-Duplicate - Inserted first", "005-CRUD", "BANK-5");
            var dataSecond = new BankAccountData("Update-Error-Duplicate - Inserted second", "006-CRUD", "BANK-6");

            EnsureEntitiesExist(dataFirst, dataSecond);

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            using (var scope = GetLocalScope())
            {
                var manager = scope.Resolve<BankAccountManager>();

                var entity = manager.AssertGetByKeyData(dataFirst.AccountName);

                entity.AccountName = dataSecond.AccountName;

                errors = manager.TryUpdate(entity).ToList();
            }

            // Assert ----------------------------

            errors.Should().ContainErrorMessage(BankAccountManager.duplicateByAccountNameError);
        }

        [Fact]
        public void TryUpdate_UpdatesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new BankAccountData("Update-Success-Valid - Inserted", "002-CRUD", "BANK-2");
            var update = new BankAccountData("Update-Success-Valid - Updated", "002-CRUD", "BANK-2");

            EnsureEntitiesExist(data);
            EnsureEntitiesDoNotExist(update);

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            using (var scope = GetLocalScope())
            {
                var manager = scope.Resolve<BankAccountManager>();

                var entity = manager.AssertGetByKeyData(data.AccountName);

                entity.AccountName = update.AccountName;

                errors = manager.TryUpdate(entity).ToList();

                manager.SaveChanges();
            }

            // Assert ----------------------------

            errors.Should().BeEmpty();

            AssertEntitiesExist(update);
        }

        private void AssertEntitiesDoNotExist(params BankAccountData[] data)
        {
            using (var scope = GetLocalScope())
            {
                var managerHelper = scope.Resolve<BankAccountManagerHelper>();

                managerHelper.AssertEntitiesDoNotExist(data);
            }
        }

        private void AssertEntitiesExist(params BankAccountData[] data)
        {
            using (var scope = GetLocalScope())
            {
                var managerHelper = scope.Resolve<BankAccountManagerHelper>();

                managerHelper.AssertEntitiesExist(data);
            }
        }

        private void EnsureEntitiesDoNotExist(params BankAccountData[] data)
        {
            using (var scope = GetLocalScope())
            {
                var managerHelper = scope.Resolve<BankAccountManagerHelper>();

                managerHelper.EnsureEntitiesDoNotExist(data);
            }
        }

        private void EnsureEntitiesExist(params BankAccountData[] data)
        {
            using (var scope = GetLocalScope())
            {
                var managerHelper = scope.Resolve<BankAccountManagerHelper>();

                managerHelper.EnsureEntitiesExist(data);
            }
        }

        private ILifetimeScope GetLocalScope(IContainer scope = null)
        {
            IContainer container = scope ?? _container;

            return container.BeginLifetimeScope();
        }

        private IContainer SetupContainer(TransactionsDbSetupHelper dbHelper)
        {
            var autofacHelper = new TransactionsAutofacSetupHelper(dbHelper);

            var builder = new ContainerBuilder();

            autofacHelper.SetupContainer(builder);

            IContainer container = builder.Build();

            return container;
        }

        private TransactionsDbSetupHelper SetupDatabase(string connectionString)
        {
            TransactionsDbSetupHelper dbHelper = new TransactionsDbSetupHelper(_connectionString);

            dbHelper.SetupDatabase();

            return dbHelper;
        }
    }
}
