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
            var dbSetup = new BudgetDbSetup();

            Helper = new BudgetManagerTestHelper(dbSetup);
        }

        public BudgetManagerTestHelper Helper { get; set; }

        [Fact]
        public void TryDelete_DeletesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new BudgetClassData("Delete test - Inserted");

            Helper.AssertEntitiesDoNotExist(data);

            Helper.AssertInsert(data.CreateEntity());

            var entity = Helper.GetEntity(data);

            // Act -------------------------------

            var errors = Helper.TryDelete(entity);

            // Assert ----------------------------

            errors.Should().BeEmpty();

            BudgetClass saved = Helper.GetEntity(data);

            saved.Should().BeNull();
        }

        [Fact]
        public void TryInsert_Fails_WhenDuplicateData()
        {
            // Arrange ---------------------------

            var data = new BudgetClassData("Duplicate Insert test - Inserted");

            Helper.AssertEntitiesDoNotExist(data);

            Helper.AssertInsert(data.CreateEntity());

            // Act -------------------------------

            var entity = data.CreateEntity();

            var errors = Helper.TryInsert(entity);

            // Assert ----------------------------

            errors.Should().ContainErrorMessage(BudgetClassManager.duplicateByNameError);
        }

        [Fact]
        public void TryInsert_InsertsRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new BudgetClassData("Insert test - Inserted");

            Helper.AssertEntitiesDoNotExist(data);

            // Act -------------------------------

            var entity = data.CreateEntity();

            var errors = Helper.TryInsert(entity);

            // Assert ----------------------------

            errors.Should().BeEmpty();

            BudgetClass saved = Helper.GetEntity(data);

            saved.Should().NotBeNull();
        }

        [Fact]
        public void TryUpdate_Fails_WhenDuplicateData()
        {
            // Arrange ---------------------------

            var dataFirst = new BudgetClassData("Duplicate Update test - Inserted first");
            var dataSecond = new BudgetClassData("Duplicate Update test - Inserted second");

            Helper.AssertEntitiesDoNotExist(dataFirst, dataSecond);

            Helper.AssertInsert(dataFirst.CreateEntity());
            Helper.AssertInsert(dataSecond.CreateEntity());

            // Act -------------------------------

            var entity = Helper.GetEntity(dataFirst);

            entity.Name = dataSecond.Name;

            var errors = Helper.TryUpdate(entity);

            // Assert ----------------------------

            errors.Should().ContainErrorMessage(BudgetClassManager.duplicateByNameError);
        }

        [Fact]
        public void TryUpdate_UpdatesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new BudgetClassData("Update test - Inserted");
            var update = new BudgetClassData("Update test - UPDATED");

            Helper.AssertEntitiesDoNotExist(data, update);

            Helper.AssertInsert(data.CreateEntity());

            var entity = Helper.GetEntity(data);

            // Act -------------------------------

            entity.Name = update.Name;

            var errors = Helper.TryUpdate(entity);

            // Assert ----------------------------

            errors.Should().BeEmpty();

            BudgetClass saved = Helper.GetEntity(update);

            saved.ShouldBeEquivalentTo(update, options => options.ExcludingMissingMembers());
        }
    }
}
