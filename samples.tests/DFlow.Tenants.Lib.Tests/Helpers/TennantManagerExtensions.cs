using DFlow.Tenants.Core.Model;
using DFlow.Tenants.Lib.Services;
using Domion.Lib.Extensions;
using FluentAssertions;

namespace DFlow.Tenants.Lib.Tests.Helpers
{
    public static class TenantManagerExtensions
    {
        /// <summary>
        /// Gets the entity by key data value or fails the assertion.
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="owner">Key data value</param>
        /// <returns>The entity</returns>
        public static Tenant AssertGetByKeyData(this TenantManager manager, string owner)
        {
            Tenant entity = manager.SingleOrDefault(c => c.Owner == owner);

            entity.Should().NotBeNull(@"because Tenant ""{0}"" MUST EXIST!", owner);

            return entity;
        }
    }
}
