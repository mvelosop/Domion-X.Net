using DFlow.Tenants.Core.Model;
using DFlow.Tenants.Lib.Services;
using Domion.Lib.Extensions;
using FluentAssertions;
using System.Linq;

namespace DFlow.Tenants.Lib.Tests.Extensions
{
    public static class TenantManagerExtensions
    {
        public static Tenant AssertGetByKeyData(this TenantManager manager, string owner)
        {
            Tenant entity = manager.SingleOrDefault(t => t.Owner == owner);

            entity.Should().NotBeNull(@"because Tenant ""{0}"" MUST EXIST!", owner);

            return entity;
        }
    }
}
