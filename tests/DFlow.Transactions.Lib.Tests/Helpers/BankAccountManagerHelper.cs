using Autofac;
using DFlow.Transactions.Core.Model;
using DFlow.Transactions.Lib.Services;
using Domion.Lib.Extensions;
using FluentAssertions;
using FluentAssertions.Equivalency;
using System;
using System.Linq;

namespace DFlow.Transactions.Lib.Tests.Helpers
{
    /// <summary>
    ///     <para>
    ///         Test helper class for BankAccountManager.
    ///     </para>
    ///     <para>
    ///         Has to be used within an Autofac ILifetimeScope. Manages entity class "BankAccount" using data class "BankAccountData" as input
    ///     </para>
    /// </summary>
    public class BankAccountManagerHelper
    {
        private Func<EquivalencyAssertionOptions<BankAccountData>, EquivalencyAssertionOptions<BankAccountData>> _dataEquivalenceOptions =
            options => options
                .Excluding(si => si.SelectedMemberPath.EndsWith("_Id"));

        private Lazy<BankAccountManager> _lazyBankAccountManager;

        private ILifetimeScope _scope;

        /// <summary>
        /// Creates the test helper for BankAccountManager
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="lazyBankAccountManager"></param>
        public BankAccountManagerHelper(
            ILifetimeScope scope,
            Lazy<BankAccountManager> lazyBankAccountManager)
        {
            _scope = scope;

            _lazyBankAccountManager = lazyBankAccountManager;
        }

        private BankAccountManager BankAccountManager { get { return _lazyBankAccountManager.Value; } }

        /// <summary>
        /// Asserts that entities with the supplied key data values do not exist
        /// </summary>
        /// <param name="dataSet"></param>
        public void AssertEntitiesDoNotExist(params BankAccountData[] dataSet)
        {
            using (var scope = GetLocalScope())
            {
                var manager = scope.Resolve<BankAccountManager>();

                foreach (var data in dataSet)
                {
                    var entity = manager.SingleOrDefault(e => e.AccountName == data.AccountName);

                    entity.Should().BeNull(@"because BankAccount ""{0}"" MUST NOT EXIST!", data.AccountName);
                }
            }
        }

        /// <summary>
        /// Asserts that entities equivalent to the supplied input data classes exist
        /// </summary>
        /// <param name="dataSet"></param>
        public void AssertEntitiesExist(params BankAccountData[] dataSet)
        {
            using (var scope = GetLocalScope())
            {
                var manager = scope.Resolve<BankAccountManager>();

                foreach (var data in dataSet)
                {
                    BankAccount entity = manager.SingleOrDefault(e => e.AccountName == data.AccountName);

                    entity.Should().NotBeNull(@"because BankAccount ""{0}"" MUST EXIST!", data.AccountName);

                    var entityData = new BankAccountData(entity);

                    entityData.ShouldBeEquivalentTo(data, options => _dataEquivalenceOptions(options));
                }
            }
        }

        /// <summary>
        /// Ensures that the entities do not exist in the database or are succesfully removed
        /// </summary>
        /// <param name="dataSet">Data for the entities to be searched and removed</param>
        public void EnsureEntitiesDoNotExist(params BankAccountData[] dataSet)
        {
            foreach (var data in dataSet)
            {
                var entity = BankAccountManager.SingleOrDefault(e => e.AccountName == data.AccountName);

                if (entity != null)
                {
                    var errors = BankAccountManager.TryDelete(entity);

                    errors.Should().BeEmpty(@"because BankAccount ""{0}"" has to be removed!", data.AccountName);
                }
            }

            BankAccountManager.SaveChanges();

            AssertEntitiesDoNotExist(dataSet);
        }

        /// <summary>
        /// Ensures that the entities exist in the database or are succesfully added
        /// </summary>
        /// <param name="dataSet"></param>
        public void EnsureEntitiesExist(params BankAccountData[] dataSet)
        {
            foreach (var data in dataSet)
            {
                var entity = BankAccountManager.SingleOrDefault(e => e.AccountName == data.AccountName);

                if (entity == null)
                {
                    entity = data.CreateEntity();

                    var errors = BankAccountManager.TryInsert(entity);

                    errors.Should().BeEmpty(@"because BankAccount ""{0}"" has to be added!", data.AccountName);
                }
            }

            BankAccountManager.SaveChanges();

            AssertEntitiesExist(dataSet);
        }

        private ILifetimeScope GetLocalScope()
        {
            return _scope.BeginLifetimeScope();
        }
    }
}
