using Demo.Transactions.Lib.Services;
using Demo.Transactions.Lib.Tests.Helpers;
using Domion.FluentAssertions.Extensions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Demo.Transactions.Lib.Tests.Tests
{
    [Trait("Type", "Integration")]
    public class TagManager_IntegrationTests
    {
        public TagManager_IntegrationTests()
        {
            var dbSetupHelper = new TransactionsDbSetupHelper();

            ManagerHelper = new TagManagerTestHelper(dbSetupHelper);
        }

        public TagManagerTestHelper ManagerHelper { get; private set; }

        [Fact]
        public void TryDelete_DeletesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new TagData("Delete test - Inserted");

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

            var data = new TagData("Duplicate Insert test - Inserted");

            ManagerHelper.AssertEntitiesDoNotExist(data);

            ManagerHelper.AssertInsert(data.CreateEntity());

            // Act -------------------------------

            var entity = data.CreateEntity();

            var errors = ManagerHelper.TryInsert(entity);

            // Assert ----------------------------

            errors.Should().ContainErrorMessage(TagManager.duplicateByNameError, "tag name can't be duplicated.");
        }

        [Fact]
        public void TryInsert_InsertsRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new TagData("Insert test - Inserted");

            ManagerHelper.AssertEntitiesDoNotExist(data);

            // Act -------------------------------

            var entity = data.CreateEntity();

            var errors = ManagerHelper.TryInsert(entity);

            // Assert ----------------------------

            errors.Should().BeEmpty();

            var saved = ManagerHelper.GetEntity(data);

            saved.Should().ShouldBeEquivalentTo(data, options => options.ExcludingMissingMembers(), "tag name can't be duplicated.");
        }

        [Fact]
        public void TryUpdate_Fails_WhenDuplicateData()
        {
            // Arrange ---------------------------

            var dataFirst = new TagData("Duplicate Update test - Inserted first");
            var dataSecond = new TagData("Duplicate Update test - Inserted second");

            ManagerHelper.AssertEntitiesDoNotExist(dataFirst, dataSecond);

            ManagerHelper.AssertInsert(dataFirst.CreateEntity());
            ManagerHelper.AssertInsert(dataSecond.CreateEntity());

            // Act -------------------------------

            var entity = ManagerHelper.GetEntity(dataFirst);

            entity.Name = dataSecond.Name;

            var errors = ManagerHelper.TryUpdate(entity);

            // Assert ----------------------------

            errors.Should().ContainErrorMessage(TagManager.duplicateByNameError);
        }

        [Fact]
        public void TryUpdate_UpdatesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new TagData("Update test - Inserted");
            var update = new TagData("Update test - UPDATED");

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
