using FluentAssertions;
using FluentAssertions.Equivalency;
using DFlow.Tenants.Core.Model;
using DFlow.Tenants.Lib.Services;
using System;
using Autofac;
using Domion.Lib.Extensions;
using DFlow.Tenants.Lib.Tests.Helpers;

namespace DFlow.Tenants.Lib.Tests.Helpers
{
    /// <summary>
    ///     <para>
    ///         Test helper class for TenantManager.
    ///     </para>
    ///
    ///     <para>
    ///         Has to be used within an Autofac ILifetimeScope. Manages entity class "Tenant" using data class "TenantData" as input
    ///     </para>
    /// </summary>
    public class TenantManagerHelper
    {
        private Func<EquivalencyAssertionOptions<TenantData>, EquivalencyAssertionOptions<TenantData>> _dataEquivalenceOptions =
            options => options
                .Excluding(si => si.SelectedMemberPath.EndsWith("_Id"));

        private Lazy<TenantManager> _lazyTenantManager;

        private ILifetimeScope _scope;

        /// <summary>
        /// Creates the test helper for TenantManager
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="lazyTenantManager"></param>
        public TenantManagerHelper(
            ILifetimeScope scope,
            Lazy<TenantManager> lazyTenantManager)
        {
            _scope = scope;

            _lazyTenantManager = lazyTenantManager;
        }

        private TenantManager TenantManager { get { return _lazyTenantManager.Value; } }

        /// <summary>
        /// Asserts that entities with the supplied key data values do not exist. Does NOT DELETE entities, use EnsureEntitiesDoNotExist for that.
        /// </summary>
        /// <param name="dataSet"></param>
        public void AssertEntitiesDoNotExist(params TenantData[] dataSet)
        {
            using (var scope = GetLocalScope())
            {
                var manager = scope.Resolve<TenantManager>();

                foreach (var data in dataSet)
                {
                    var entity = manager.SingleOrDefault(e => e.Owner == data.Owner);

                    entity.Should().BeNull(@"because Tenant ""{0}"" MUST NOT EXIST!", data.Owner);
                }
            }
        }

        /// <summary>
        /// Asserts that entities equivalent to the supplied input data classes exist. Does NOT CREATE OR UPDATE entities, use EnsureEntitiesExist for that.
        /// </summary>
        /// <param name="dataSet"></param>
        public void AssertEntitiesExist(params TenantData[] dataSet)
        {
            using (var scope = GetLocalScope())
            {
                var manager = scope.Resolve<TenantManager>();

                foreach (var data in dataSet)
                {
                    Tenant entity = manager.SingleOrDefault(e => e.Owner == data.Owner);

                    entity.Should().NotBeNull(@"because Tenant ""{0}"" MUST EXIST!", data.Owner);

                    var entityData = new TenantData(entity);

                    entityData.ShouldBeEquivalentTo(data, options => _dataEquivalenceOptions(options));
                }
            }
        }

        /// <summary>
        /// Ensures that the entities do not exist in the database or are successfully removed.
        /// </summary>
        /// <param name="dataSet">Data for the entities to be searched and removed</param>
        public void EnsureEntitiesDoNotExist(params TenantData[] dataSet)
        {
            foreach (var data in dataSet)
            {
                var entity = TenantManager.SingleOrDefault(e => e.Owner == data.Owner);

                if (entity != null)
                {
                    var errors = TenantManager.TryDelete(entity);

                    errors.Should().BeEmpty(@"because Tenant ""{0}"" has to be removed!", data.Owner);
                }
            }

            TenantManager.SaveChanges();

            AssertEntitiesDoNotExist(dataSet);
        }

        /// <summary>
        /// Ensures that the entities exist in the database or are successfully added.
        /// </summary>
        /// <param name="dataSet"></param>
        public void EnsureEntitiesExist(params TenantData[] dataSet)
        {
            foreach (var data in dataSet)
            {
                var entity = TenantManager.SingleOrDefault(e => e.Owner == data.Owner);

                if (entity == null)
                {
                    entity = data.CreateEntity();

                    var errors = TenantManager.TryInsert(entity);

                    errors.Should().BeEmpty(@"because Tenant ""{0}"" has to be added!", data.Owner);
                }
            }

            TenantManager.SaveChanges();

            AssertEntitiesExist(dataSet);
        }

        private ILifetimeScope GetLocalScope()
        {
            return _scope.BeginLifetimeScope();
        }
    }
}
