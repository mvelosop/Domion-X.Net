using DFlow.Budget.Lib.Services;
using DFlow.Budget.Lib.Tests.Helpers;
using Domion.FluentAssertions.Extensions;
using FluentAssertions;
using Xunit;

namespace DFlow.Budget.Lib.Tests
{
    [Trait("Type", "Integration")]
    public class BudgetClassManager_IntegrationTests
    {
        public BudgetClassManager_IntegrationTests()
        {
            var dbHelper = new BudgetDbSetupHelper();

            dbHelper.SetupDatabase();

            ManagerHelper = new BudgetClassManagerHelper(dbHelper);
        }

        public BudgetClassManagerHelper ManagerHelper { get; private set; }

        [Fact]
        public void TryDelete_DeletesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new BudgetClassData("Delete test - Inserted", "Income");

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

            var data = new BudgetClassData("Duplicate Insert test - Inserted", "Income");

            ManagerHelper.AssertEntitiesDoNotExist(data);

            ManagerHelper.AssertInsert(data.CreateEntity());

            // Act -------------------------------

            var entity = data.CreateEntity();

            var errors = ManagerHelper.TryInsert(entity);

            // Assert ----------------------------

            errors.Should().ContainErrorMessage(BudgetClassManager.duplicateByNameError);
        }

        [Fact]
        public void TryInsert_InsertsRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new BudgetClassData("Insert test - Inserted", "Income");

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

            var dataFirst = new BudgetClassData("Duplicate Update test - Inserted first", "Income");
            var dataSecond = new BudgetClassData("Duplicate Update test - Inserted second", "Income");

            ManagerHelper.AssertEntitiesDoNotExist(dataFirst, dataSecond);

            ManagerHelper.AssertInsert(dataFirst.CreateEntity());
            ManagerHelper.AssertInsert(dataSecond.CreateEntity());

            // Act -------------------------------

            var entity = ManagerHelper.GetEntity(dataFirst);

            entity.Name = dataSecond.Name;

            var errors = ManagerHelper.TryUpdate(entity);

            // Assert ----------------------------

            errors.Should().ContainErrorMessage(BudgetClassManager.duplicateByNameError);
        }

        [Fact]
        public void TryUpdate_UpdatesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new BudgetClassData("Update test - Inserted", "Income");
            var update = new BudgetClassData("Update test - UPDATED", "Income");

            ManagerHelper.AssertEntitiesDoNotExist(data, update);

            ManagerHelper.AssertInsert(data.CreateEntity());

            var entity = ManagerHelper.GetEntity(data);

            // Act -------------------------------

            entity.Name = update.Name;

            var errors = ManagerHelper.TryUpdate(entity);

            // Assert ----------------------------

            errors.Should().BeEmpty();

            var saved = ManagerHelper.GetEntity(update);

            saved.ShouldBeEquivalentTo(update, options => options.ExcludingMissingMembers());
        }
    }
}
