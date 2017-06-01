using Demo.Transactions.Lib.Services;
using Demo.Transactions.Lib.Tests.Helpers;
using Domion.FluentAssertions.Extensions;
using FluentAssertions;
using Xunit;

namespace Demo.Transactions.Lib.Tests.Tests
{
    [Trait("Type", "Integration")]
    public class BankAccountManager_IntegrationTests
    {
        public BankAccountManager_IntegrationTests()
        {
            var dbSetupHelper = new TransactionsDbSetupHelper();

            ManagerHelper = new BankAccountManagerTestHelper(dbSetupHelper);
        }

        public BankAccountManagerTestHelper ManagerHelper { get; private set; }

        [Fact]
        public void TryDelete_DeletesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new BankAccountData("003-CRUD", "Delete test - Inserted");

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

            var data = new BankAccountData("004-CRUD", "Duplicate Insert test - Inserted");

            ManagerHelper.AssertEntitiesDoNotExist(data);

            ManagerHelper.AssertInsert(data.CreateEntity());

            // Act -------------------------------

            var entity = data.CreateEntity();

            var errors = ManagerHelper.TryInsert(entity);

            // Assert ----------------------------

            errors.Should().ContainErrorMessage(BankAccountManager.duplicateByAccountNumberError, "account number can't be duplicated.");
        }

        [Fact]
        public void TryInsert_InsertsRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new BankAccountData("001-CRUD", "Insert test - Inserted");

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

            var dataFirst = new BankAccountData("005-CRUD", "Duplicate Update test - Inserted first");
            var dataSecond = new BankAccountData("006-CRUD", "Duplicate Update test - Inserted second");

            ManagerHelper.AssertEntitiesDoNotExist(dataFirst, dataSecond);

            ManagerHelper.AssertInsert(dataFirst.CreateEntity());
            ManagerHelper.AssertInsert(dataSecond.CreateEntity());

            // Act -------------------------------

            var entity = ManagerHelper.GetEntity(dataFirst);

            entity.AccountNumber = dataSecond.AccountNumber;

            var errors = ManagerHelper.TryUpdate(entity);

            // Assert ----------------------------

            errors.Should().ContainErrorMessage(BankAccountManager.duplicateByAccountNumberError, "account number can't be duplicated.");
        }

        [Fact]
        public void TryUpdate_UpdatesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new BankAccountData("002-CRUD", "Update test - Inserted");
            var update = new BankAccountData("002-CRUD", "Update test - UPDATED");

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
