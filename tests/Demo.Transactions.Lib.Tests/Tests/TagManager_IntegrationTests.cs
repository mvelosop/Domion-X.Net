using Demo.Transactions.Lib.Tests.Helpers;
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

            saved.Should().ShouldBeEquivalentTo(data, options => options.ExcludingMissingMembers());
        }
    }
}
