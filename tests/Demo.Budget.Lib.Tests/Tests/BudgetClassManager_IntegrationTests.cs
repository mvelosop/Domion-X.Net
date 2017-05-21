using Demo.Budget.Core.Model;
using Demo.Budget.Lib.Services;
using Demo.Budget.Lib.Tests.Helpers;
using Domion.FluentAssertions.Extensions;
using FluentAssertions;
using Xunit;

namespace Demo.Budget.Lib.Tests
{
    [Trait("Type", "Integration")]
    public class BudgetClassManager_IntegrationTests
    {
        public BudgetClassManager_IntegrationTests()
        {
            var dbSetup = new BudgetDbSetupHelper();

            ManagerHelper = new BudgetManagerTestHelper(dbSetup);
        }

        public BudgetManagerTestHelper ManagerHelper { get; private set; }

        [Fact]
        public void TryDelete_DeletesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new BudgetClassData("Delete test - Inserted");

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

            var data = new BudgetClassData("Duplicate Insert test - Inserted");

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

            var data = new BudgetClassData("Insert test - Inserted");

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

            var dataFirst = new BudgetClassData("Duplicate Update test - Inserted first");
            var dataSecond = new BudgetClassData("Duplicate Update test - Inserted second");

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

            var data = new BudgetClassData("Update test - Inserted");
            var update = new BudgetClassData("Update test - UPDATED");

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
