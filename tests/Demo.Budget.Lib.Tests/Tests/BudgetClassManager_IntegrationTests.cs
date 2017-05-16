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
            DbSetup = new BudgetDbSetup();
        }

        public BudgetDbSetup DbSetup { get; set; }

        [Fact]
        public void TryDelete_DeletesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new BudgetClassData("Delete test - Inserted");

            DbSetup.AssertEntitiesDoNotExist(data);

            DbSetup.AssertInsert(data.CreateEntity());

            var entity = DbSetup.GetEntity(data);

            // Act -------------------------------

            var errors = DbSetup.TryDelete(entity);

            // Assert ----------------------------

            errors.Should().BeEmpty();

            BudgetClass saved = DbSetup.GetEntity(data);

            saved.Should().BeNull();
        }

        [Fact]
        public void TryInsert_Fails_WhenDuplicateData()
        {
            // Arrange ---------------------------

            var data = new BudgetClassData("Duplicate Insert test - Inserted");

            DbSetup.AssertEntitiesDoNotExist(data);

            DbSetup.AssertInsert(data.CreateEntity());

            // Act -------------------------------

            var entity = data.CreateEntity();

            var errors = DbSetup.TryInsert(entity);

            // Assert ----------------------------

            errors.Should().ContainErrorMessage(BudgetClassManager.duplicateByNameError);
        }

        [Fact]
        public void TryInsert_InsertsRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new BudgetClassData("Insert test - Inserted");

            DbSetup.AssertEntitiesDoNotExist(data);

            // Act -------------------------------

            var entity = data.CreateEntity();

            var errors = DbSetup.TryInsert(entity);

            // Assert ----------------------------

            errors.Should().BeEmpty();

            BudgetClass saved = DbSetup.GetEntity(data);

            saved.Should().NotBeNull();
        }

        [Fact]
        public void TryUpdate_Fails_WhenDuplicateData()
        {
            // Arrange ---------------------------

            var dataFirst = new BudgetClassData("Duplicate Update test - Inserted first");
            var dataSecond = new BudgetClassData("Duplicate Update test - Inserted second");

            DbSetup.AssertEntitiesDoNotExist(dataFirst, dataSecond);

            DbSetup.AssertInsert(dataFirst.CreateEntity());
            DbSetup.AssertInsert(dataSecond.CreateEntity());

            // Act -------------------------------

            var entity = DbSetup.GetEntity(dataFirst);

            entity.Name = dataSecond.Name;

            var errors = DbSetup.TryUpdate(entity);

            // Assert ----------------------------

            errors.Should().ContainErrorMessage(BudgetClassManager.duplicateByNameError);
        }

        [Fact]
        public void TryUpdate_UpdatesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new BudgetClassData("Update test - Inserted");
            var update = new BudgetClassData("Update test - UPDATED");

            DbSetup.AssertEntitiesDoNotExist(data, update);

            DbSetup.AssertInsert(data.CreateEntity());

            var entity = DbSetup.GetEntity(data);

            // Act -------------------------------

            entity.Name = update.Name;

            var errors = DbSetup.TryUpdate(entity);

            // Assert ----------------------------

            errors.Should().BeEmpty();

            BudgetClass saved = DbSetup.GetEntity(update);

            saved.ShouldBeEquivalentTo(update, options => options.ExcludingMissingMembers());
        }
    }
}
