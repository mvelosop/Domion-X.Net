using Demo.Budget.Core.Model;
using Demo.Budget.Lib.Tests.Helpers;
using FluentAssertions;
using Xunit;

namespace Demo.Budget.Lib.Tests
{
    public class BudgetClassManager_IntegrationTests
    {
        public BudgetClassManager_IntegrationTests()
        {
            DbSetup = new BudgetDbSetup();
        }

        public BudgetDbSetup DbSetup { get; set; }

        [Fact]
        public void TryInsert_InsertsRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new BudgetClassData("Insert test - Inserted");

            DbSetup.AssertEntitiesDoesNotExist(data);

            // Act -------------------------------

            var entity = data.CreateEntity();

            DbSetup.TryInsert(entity);

            // Assert ----------------------------

            BudgetClass saved = DbSetup.GetEntity(data);

            saved.Should().NotBeNull();
        }

        [Fact]
        public void TryUpdate_UpdatesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new BudgetClassData("Update test - Inserted");
            var update = new BudgetClassData("Update test - UPDATED");

            DbSetup.AssertEntitiesDoesNotExist(data, update);

            DbSetup.TryInsert(data.CreateEntity());

            var entity = DbSetup.GetEntity(data);

            // Act -------------------------------

            entity.Name = update.Name;

            DbSetup.TryUpdate(entity);

            // Assert ----------------------------

            BudgetClass saved = DbSetup.GetEntity(update);

            saved.ShouldBeEquivalentTo(update, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void TryDelete_DeletesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new BudgetClassData("Delete test - Inserted");

            DbSetup.AssertEntitiesDoesNotExist(data);

            DbSetup.TryInsert(data.CreateEntity());

            var entity = DbSetup.GetEntity(data);

            // Act -------------------------------

            DbSetup.TryDelete(entity);

            // Assert ----------------------------

            BudgetClass saved = DbSetup.GetEntity(data);

            saved.Should().BeNull();
        }
    }
}
