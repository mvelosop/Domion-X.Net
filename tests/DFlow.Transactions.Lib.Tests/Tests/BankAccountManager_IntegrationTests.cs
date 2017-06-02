using DFlow.Transactions.Lib.Services;
using DFlow.Transactions.Lib.Tests.Helpers;
using Domion.FluentAssertions.Extensions;
using FluentAssertions;
using Xunit;

namespace DFlow.Transactions.Lib.Tests.Tests
{
    [Trait("Type", "Integration")]
    public class BankAccountManager_IntegrationTests
    {
        public BankAccountManager_IntegrationTests()
        {
            var dbHelper = new TransactionsDbSetupHelper();

            dbHelper.SetupDatabase();

            ManagerHelper = new BankAccountManagerTestHelper(dbHelper);
        }

        public BankAccountManagerTestHelper ManagerHelper { get; private set; }

        [Fact]
        public void TryDelete_DeletesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new BankAccountData("BANK-3", "003-CRUD", "Delete test - Inserted");

            ManagerHelper.AssertEntitiesDoNotExist(data);

            ManagerHelper.AssertInsert(data.CreateEntity());

            var entity = ManagerHelper.GetEntity(data);

            // Act -------------------------------

            var errors = ManagerHelper.TryDelete(entity);

            // Assert ----------------------------

            errors.Should().BeEmpty();

            var saved = ManagerHelper.GetEntity(data);

            saved.Should().BeNull();
        }

        [Fact]
        public void TryInsert_Fails_WhenDuplicateData()
        {
            // Arrange ---------------------------

            var data = new BankAccountData("BANK-4", "004-CRUD", "Duplicate Insert test - Inserted");

            ManagerHelper.AssertEntitiesDoNotExist(data);

            ManagerHelper.AssertInsert(data.CreateEntity());

            // Act -------------------------------

            var entity = data.CreateEntity();

            var errors = ManagerHelper.TryInsert(entity);

            // Assert ----------------------------

            errors.Should().ContainErrorMessage(BankAccountManager.duplicateByAccountNameError, "account name can't be duplicated.");
        }

        [Fact]
        public void TryInsert_InsertsRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new BankAccountData("BANK-1", "001-CRUD", "Insert test - Inserted");

            ManagerHelper.AssertEntitiesDoNotExist(data);

            // Act -------------------------------

            var entity = data.CreateEntity();

            var errors = ManagerHelper.TryInsert(entity);

            // Assert ----------------------------

            errors.Should().BeEmpty();

            var saved = ManagerHelper.GetEntity(data);

            saved.Should().ShouldBeEquivalentTo(data, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void TryUpdate_Fails_WhenDuplicateData()
        {
            // Arrange ---------------------------

            var dataFirst = new BankAccountData("BANK-5", "005-CRUD", "Duplicate Update test - Inserted first");
            var dataSecond = new BankAccountData("BANK-6", "006-CRUD", "Duplicate Update test - Inserted second");

            ManagerHelper.AssertEntitiesDoNotExist(dataFirst, dataSecond);

            ManagerHelper.AssertInsert(dataFirst.CreateEntity());
            ManagerHelper.AssertInsert(dataSecond.CreateEntity());

            // Act -------------------------------

            var entity = ManagerHelper.GetEntity(dataFirst);

            entity.AccountName = dataSecond.AccountName;

            var errors = ManagerHelper.TryUpdate(entity);

            // Assert ----------------------------

            errors.Should().ContainErrorMessage(BankAccountManager.duplicateByAccountNameError, "account name can't be duplicated.");
        }

        [Fact]
        public void TryUpdate_UpdatesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new BankAccountData("BANK-2", "002-CRUD", "Update test - Inserted");
            var update = new BankAccountData("BANK-2", "002-CRUD", "Update test - UPDATED");

            ManagerHelper.AssertEntitiesDoNotExist(data, update);

            ManagerHelper.AssertInsert(data.CreateEntity());

            var entity = ManagerHelper.GetEntity(data);

            // Act -------------------------------

            entity.BankName = update.BankName;

            var errors = ManagerHelper.TryUpdate(entity);

            // Assert ----------------------------

            errors.Should().BeEmpty();

            var saved = ManagerHelper.GetEntity(update);

            saved.ShouldBeEquivalentTo(update, options => options.ExcludingMissingMembers());
        }
    }
}
