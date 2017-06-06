//----------------------------------------
// DFlow               (e.g. DFlow)
// Tennants            (e.g. Budget)
// Tennant            (e.g. BudgetClass)
// Owner (e.g. Name)
//----------------------------------------

using FluentAssertions;
using FluentAssertions.Equivalency;
using DFlow.Tennants.Core.Model;
using DFlow.Tennants.Lib.Services;
using System;
using Autofac;
using Domion.Lib.Extensions;

namespace DFlow.Tennants.Lib.Tests.Helpers
{
	/// <summary>
	///     <para>
	///         Test helper class for TennantManager.
	///     </para>
	///
	///     <para>
	///         Has to be used within an Autofac ILifetimeScope. Manages entity class "Tennant" using data class "TennantData" as input
	///     </para>
	/// </summary>
	public class TennantManagerHelper
	{
		private Func<EquivalencyAssertionOptions<TennantData>, EquivalencyAssertionOptions<TennantData>> _dataEquivalenceOptions =
			options => options
				.Excluding(si => si.SelectedMemberPath.EndsWith("_Id"));

		private Lazy<TennantManager> _lazyTennantManager;

		private ILifetimeScope _scope;

		/// <summary>
		/// Creates the test helper for TennantManager
		/// </summary>
		/// <param name="scope"></param>
		/// <param name="lazyTennantManager"></param>
		public TennantManagerHelper(
			ILifetimeScope scope,
			Lazy<TennantManager> lazyTennantManager)
		{
			_scope = scope;

			_lazyTennantManager = lazyTennantManager;
		}

		private TennantManager TennantManager { get { return _lazyTennantManager.Value; } }

		/// <summary>
		/// Asserts that entities with the supplied key data values do not exist
		/// </summary>
		/// <param name="dataSet"></param>
		public void AssertEntitiesDoNotExist(params TennantData[] dataSet)
		{
			using (var scope = GetLocalScope())
			{
				var manager = scope.Resolve<TennantManager>();

				foreach (var data in dataSet)
				{
					var entity = manager.SingleOrDefault(e => e.Owner == data.Owner);

					entity.Should().BeNull(@"because Tennant ""{0}"" MUST NOT EXIST!", data.Owner);
				}
			}
		}

		/// <summary>
		/// Asserts that entities equivalent to the supplied input data classes exist
		/// </summary>
		/// <param name="dataSet"></param>
		public void AssertEntitiesExist(params TennantData[] dataSet)
		{
			using (var scope = GetLocalScope())
			{
				var manager = scope.Resolve<TennantManager>();

				foreach (var data in dataSet)
				{
					Tennant entity = manager.SingleOrDefault(e => e.Owner == data.Owner);

					entity.Should().NotBeNull(@"because Tennant ""{0}"" MUST EXIST!", data.Owner);

					var entityData = new TennantData(entity);

					entityData.ShouldBeEquivalentTo(data, options => _dataEquivalenceOptions(options));
				}
			}
		}

		/// <summary>
		/// Ensures that the entities do not exist in the database or are succesfully removed
		/// </summary>
		/// <param name="dataSet">Data for the entities to be searched and removed</param>
		public void EnsureEntitiesDoNotExist(params TennantData[] dataSet)
		{
			foreach (var data in dataSet)
			{
				var entity = TennantManager.SingleOrDefault(e => e.Owner == data.Owner);

				if (entity != null)
				{
					var errors = TennantManager.TryDelete(entity);

					errors.Should().BeEmpty(@"because Tennant ""{0}"" has to be removed!", data.Owner);
				}
			}

			TennantManager.SaveChanges();

			AssertEntitiesDoNotExist(dataSet);
		}

		/// <summary>
		/// Ensures that the entities exist in the database or are succesfully added
		/// </summary>
		/// <param name="dataSet"></param>
		public void EnsureEntitiesExist(params TennantData[] dataSet)
		{
			foreach (var data in dataSet)
			{
				var entity = TennantManager.SingleOrDefault(e => e.Owner == data.Owner);

				if (entity == null)
				{
					entity = data.CreateEntity();

					var errors = TennantManager.TryInsert(entity);

					errors.Should().BeEmpty(@"because Tennant ""{0}"" has to be added!", data.Owner);
				}
			}

			TennantManager.SaveChanges();

			AssertEntitiesExist(dataSet);
		}

		private ILifetimeScope GetLocalScope()
		{
			return _scope.BeginLifetimeScope();
		}
	}
}
