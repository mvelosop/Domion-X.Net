using DFlow.Tenants.Core.Model;
using DFlow.Tenants.Lib.Services;
using Domion.Lib.Extensions;
using FluentAssertions;
using System.Linq;

namespace DFlow.Tenants.Lib.Tests.Extensions
{
    public static class TenantRepositoryExtensions
    {
        public static Tenant AssertGetByKeyData(this TenantRepository repo, string owner)
        {
            Tenant entity = repo.SingleOrDefault(t => t.Owner == owner);

            entity.Should().NotBeNull(@"because Tenant ""{0}"" MUST EXIST!", owner);

            return entity;
        }
    }
}
