using DFlow.Tennants.Core.Model;
using DFlow.Tennants.Lib.Services;
using Domion.Lib.Extensions;
using FluentAssertions;

namespace DFlow.Tennants.Lib.Tests.Helpers
{
    public static class TennantManagerExtensions
    {
        /// <summary>
        /// Gets the entity by key data value or fails the assertion.
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="owner">Key data value</param>
        /// <returns>The entity</returns>
        public static Tennant AssertGetByKeyData(this TennantManager manager, string owner)
        {
            Tennant entity = manager.SingleOrDefault(c => c.Owner == owner);

            entity.Should().NotBeNull(@"because Tennant ""{0}"" MUST EXIST!", owner);

            return entity;
        }
    }
}
