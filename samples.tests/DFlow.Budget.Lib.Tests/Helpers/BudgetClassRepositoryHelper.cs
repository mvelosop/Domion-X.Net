using Autofac;
using DFlow.Budget.Core.Model;
using DFlow.Budget.Lib.Services;
using Domion.Lib.Extensions;
using FluentAssertions;
using System;

namespace DFlow.Budget.Lib.Tests.Helpers
{
    /// <summary>
    ///     <para>
    ///         Test helper class for BudgetClassRepository.
    ///     </para>
    ///
    ///     <para>
    ///         Has to be used within an Autofac ILifetimeScope. Manages entity class "BudgetClass" using data class "BudgetClassData" as input
    ///     </para>
    /// </summary>
    public class BudgetClassRepositoryHelper
    {
        private readonly Lazy<BudgetClassDataMapper> LazyBudgetClassDataMapper;
        private readonly Lazy<BudgetClassRepository> LazyBudgetClassRepo;
        private readonly ILifetimeScope Scope;

        /// <summary>
        ///     Creates a Helper for BudgetClassRepository to help in the test's Arrange and Assert sections
        /// </summary>
        public BudgetClassRepositoryHelper(
            ILifetimeScope scope,
            Lazy<BudgetClassDataMapper> lazyBudgetClassDataMapper,
            Lazy<BudgetClassRepository> lazyBudgetClassRepo)
        {
            Scope = scope;

            LazyBudgetClassRepo = lazyBudgetClassRepo;
            LazyBudgetClassDataMapper = lazyBudgetClassDataMapper;
        }

        private BudgetClassRepository BudgetClassRepo => LazyBudgetClassRepo.Value;

        private BudgetClassDataMapper BudgetClassMapper => LazyBudgetClassDataMapper.Value;

        /// <summary>
        ///     Asserts that entities with the supplied key data values do not exist. This method DOES NOT DELETE entities, use EnsureEntitiesDoNotExist for that.
        /// </summary>
        /// <param name="dataSet">Data for the entities to be searched for</param>
        public void AssertEntitiesDoNotExist(params BudgetClassData[] dataSet)
        {
            using (ILifetimeScope scope = Scope.BeginLifetimeScope())
            {
                var repo = scope.Resolve<BudgetClassRepository>();

                foreach (BudgetClassData data in dataSet)
                {
                    BudgetClass entity = repo.SingleOrDefault(e => e.Name == data.Name);

                    entity.Should().BeNull(@"because BudgetClass ""{0}"" MUST NOT EXIST!", data.Name);
                }
            }
        }

        /// <summary>
        ///     Asserts that entities equivalent to the supplied input data classes exist. This method DOES NOT CREATE OR UPDATE entities, , use EnsureEntitiesExist for that.
        /// </summary>
        /// <param name="dataSet">Data for the entities to be searched for</param>
        public void AssertEntitiesExist(params BudgetClassData[] dataSet)
        {
            using (ILifetimeScope scope = Scope.BeginLifetimeScope())
            {
                var repo = scope.Resolve<BudgetClassRepository>();
                var mapper = scope.Resolve<BudgetClassDataMapper>();

                foreach (BudgetClassData data in dataSet)
                {
                    BudgetClass entity = repo.SingleOrDefault(e => e.Name == data.Name);

                    entity.Should().NotBeNull(@"because BudgetClass ""{0}"" MUST EXIST!", data.Name);

                    BudgetClassData entityData = mapper.CreateData(entity);

                    entityData.ShouldBeEquivalentTo(data);
                }
            }
        }

        /// <summary>
        ///     Ensures that the entities do not exist in the database or are successfully removed
        /// </summary>
        /// <param name="dataSet">Data for the entities to be searched for and removed if necessary</param>
        public void EnsureEntitiesDoNotExist(params BudgetClassData[] dataSet)
        {
            foreach (BudgetClassData data in dataSet)
            {
                BudgetClass entity = BudgetClassRepo.SingleOrDefault(e => e.Name == data.Name);

                if (entity == null) continue;

                var errors = BudgetClassRepo.TryDelete(entity);

                errors.Should().BeEmpty(@"because BudgetClass ""{0}"" has to be removed!", data.Name);
            }

            BudgetClassRepo.SaveChanges();

            AssertEntitiesDoNotExist(dataSet);
        }

        /// <summary>
        ///     Ensures that the entities exist in the database or are successfully added
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="dataSet">Data for the entities to be searched for and added or updated if necessary</param>
        public void EnsureEntitiesExist(params BudgetClassData[] dataSet)
        {
            foreach (BudgetClassData data in dataSet)
            {
                BudgetClass entity = BudgetClassRepo.SingleOrDefault(e => e.Name == data.Name);

                entity = entity == null ? BudgetClassMapper.CreateEntity(data) : BudgetClassMapper.UpdateEntity(data, entity);

                var errors = BudgetClassRepo.TryUpsert(entity);

                errors.Should().BeEmpty(@"because BudgetClass ""{0}"" has to be added!", data.Name);
            }

            BudgetClassRepo.SaveChanges();

            AssertEntitiesExist(dataSet);
        }
    }
}
