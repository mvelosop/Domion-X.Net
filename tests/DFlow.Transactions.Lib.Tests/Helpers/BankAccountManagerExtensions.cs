using DFlow.Transactions.Core.Model;
using DFlow.Transactions.Lib.Services;
using FluentAssertions;
using System.Linq;

namespace DFlow.Transactions.Lib.Tests.Helpers
{
    public static class BankAccountManagerExtensions
    {
        /// <summary>
        /// Gets the entity by key data value or fails the assertion.
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="name">Key data value</param>
        /// <returns>The entity </returns>
        public static BankAccount AssertGetByKeyData(this BankAccountManager manager, string accountName)
        {
            BankAccount entity = manager.Query(ba => ba.AccountName == accountName).SingleOrDefault();

            entity.Should().NotBeNull(@"because BankAccount ""{0}"" MUST EXIST!", accountName);

            return entity;
        }
    }
}
