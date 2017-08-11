using System;
using DFlow.Tenants.Core.Model;
using DFlow.WebApp.Features.Tenants;
using FluentAssertions;
using Xunit;

namespace DFlow.WebApp.Tests.Tests
{
    public class TenantViewModelMapper_Tests
    {
        [Theory]
        [InlineData(1, null)]
        [InlineData(2, new byte[] { })]
        public void UpdateEntity_Fails_WhenInvalidRowVersion(int testCase, byte[] rowVersion)
        {
            // Arrange ---------------------------

            var mapper = new TenantViewModelMapper();
            
            var vm = new TenantViewModel
            {
                RowVersion = rowVersion
            };

            var entity = new Tenant();
            
            // Act -------------------------------

            Action action = () => mapper.UpdateEntity(vm, entity);

            // Assert ----------------------------

            action.ShouldThrow<InvalidOperationException>().Which.Message.Should().Contain("RowVersion");
        }
    }
}
