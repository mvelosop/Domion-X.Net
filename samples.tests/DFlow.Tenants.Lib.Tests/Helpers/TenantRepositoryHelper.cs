﻿using Autofac;
using DFlow.Tenants.Core.Model;
using DFlow.Tenants.Lib.Services;
using Domion.Lib.Extensions;
using FluentAssertions;
using FluentAssertions.Equivalency;
using System;

namespace DFlow.Tenants.Lib.Tests.Helpers
{
    /// <summary>
    ///     <para>
    ///         Test helper class for TenantRepository.
    ///     </para>
    ///
    ///     <para>
    ///         Has to be used within an Autofac ILifetimeScope. Manages entity class "Tenant" using data class "TenantData" as input
    ///     </para>
    /// </summary>
    public class TenantRepositoryHelper
    {
        private readonly Lazy<TenantDataMapper> _lazyTenantDataMapper;
        private readonly Lazy<TenantRepository> _lazyTenantRepo;
        private readonly ILifetimeScope _scope;

        /// <summary>
        ///     Creates a Helper for TenantRepository to help in the test's Arrange and Assert sections
        /// </summary>
        public TenantRepositoryHelper(
            ILifetimeScope scope,
            Lazy<TenantDataMapper> lazyTenantDataMapper,
            Lazy<TenantRepository> lazyTenantRepo)
        {
            _scope = scope;

            _lazyTenantRepo = lazyTenantRepo;
            _lazyTenantDataMapper = lazyTenantDataMapper;
        }

        private TenantRepository TenantRepo => _lazyTenantRepo.Value;

        private TenantDataMapper TenantMapper => _lazyTenantDataMapper.Value;

        /// <summary>
        ///     Asserts that entities with the supplied key data values do not exist. This method DOES NOT DELETE entities, use EnsureEntitiesDoNotExist for that.
        /// </summary>
        /// <param name="dataSet">Data for the entities to be searched for</param>
        public void AssertEntitiesDoNotExist(params TenantData[] dataSet)
        {
            using (ILifetimeScope scope = _scope.BeginLifetimeScope())
            {
                var repo = scope.Resolve<TenantRepository>();

                foreach (TenantData data in dataSet)
                {
                    Tenant entity = repo.SingleOrDefault(e => e.Owner == data.Owner);

                    entity.Should().BeNull(@"because Tenant ""{0}"" MUST NOT EXIST!", data.Owner);
                }
            }
        }

        /// <summary>
        ///     Asserts that entities equivalent to the supplied input data classes exist. This method DOES NOT CREATE OR UPDATE entities, , use EnsureEntitiesExist for that.
        /// </summary>
        /// <param name="dataSet">Data for the entities to be searched for</param>
        public void AssertEntitiesExist(params TenantData[] dataSet)
        {
            using (ILifetimeScope scope = _scope.BeginLifetimeScope())
            {
                var repo = scope.Resolve<TenantRepository>();
                var mapper = scope.Resolve<TenantDataMapper>();

                foreach (TenantData data in dataSet)
                {
                    Tenant entity = repo.SingleOrDefault(e => e.Owner == data.Owner);

                    entity.Should().NotBeNull(@"because Tenant ""{0}"" MUST EXIST!", data.Owner);

                    TenantData entityData = mapper.CreateData(entity);

                    entityData.Should().BeEquivalentTo(data);
                }
            }
        }

        /// <summary>
        ///     Ensures that the entities do not exist in the database or are successfully removed
        /// </summary>
        /// <param name="dataSet">Data for the entities to be searched for and removed if necessary</param>
        public void EnsureEntitiesDoNotExist(params TenantData[] dataSet)
        {
            foreach (TenantData data in dataSet)
            {
                Tenant entity = TenantRepo.SingleOrDefault(e => e.Owner == data.Owner);

                if (entity == null) continue;

                var errors = TenantRepo.TryDelete(entity);

                errors.Should().BeEmpty(@"because Tenant ""{0}"" has to be removed!", data.Owner);
            }

            TenantRepo.SaveChanges();

            AssertEntitiesDoNotExist(dataSet);
        }

        /// <summary>
        ///     Ensures that the entities exist in the database or are successfully added
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="dataSet">Data for the entities to be searched for and added or updated if necessary</param>
        public void EnsureEntitiesExist(params TenantData[] dataSet)
        {
            foreach (TenantData data in dataSet)
            {
                Tenant entity = TenantRepo.SingleOrDefault(e => e.Owner == data.Owner);

                entity = entity == null ? TenantMapper.CreateEntity(data) : TenantMapper.UpdateEntity(data, entity);

                var errors = TenantRepo.TryUpsert(entity);

                errors.Should().BeEmpty(@"because Tenant ""{0}"" has to be added!", data.Owner);
            }

            TenantRepo.SaveChanges();

            AssertEntitiesExist(dataSet);
        }
    }
}
