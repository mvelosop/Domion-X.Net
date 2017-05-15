//------------------------------------------------------------------------------
// BudgetClassManager.cs
//
// Implementation of: BudgetClassManager (Class) <<entity-manager>>
// Generated with Domion-MDA - www.coderepo.blog
//------------------------------------------------------------------------------

using Demo.Budget.Core.Model;
using Demo.Budget.Core.Services;
using Demo.Budget.Lib.Data;
using Domion.Core.Services;
using Domion.Lib.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace Demo.Budget.Lib.Services
{
	public class BudgetClassManager : BaseRepository<BudgetClass, int>, IEntityManager<BudgetClass, int>, IBudgetClassManager
	{
		public BudgetClassManager(BudgetDbContext dbContext)
			: base(dbContext)
		{
		}

		public override IQueryable<BudgetClass> Query(Expression<Func<BudgetClass, bool>> where)
		{
			return base.Query(where);
		}

		public virtual BudgetClass Refresh(BudgetClass entity)
		{
			base.Detach(entity);

			return Find(entity.Id);
		}

		public new virtual IEnumerable<ValidationResult> TryDelete(BudgetClass entity)
		{
			return base.TryDelete(entity);
		}

		public new virtual IEnumerable<ValidationResult> TryInsert(BudgetClass entity)
		{
			if (entity.RowVersion != null && entity.RowVersion.Length > 0) throw new InvalidOperationException("RowVersion not empty on Insert");

			CommonSaveOperations(entity);

			return base.TryInsert(entity);
		}

		public new virtual IEnumerable<ValidationResult> TryUpdate(BudgetClass entity)
		{
			if (entity.RowVersion == null || entity.RowVersion.Length == 0) throw new InvalidOperationException("RowVersion empty on Update");

			CommonSaveOperations(entity);

			return base.TryUpdate(entity);
		}

		public virtual IEnumerable<ValidationResult> TryUpsert(BudgetClass entity)
		{
			if (entity.Id == 0)
			{
				return TryInsert(entity);
			}
			else
			{
				return TryUpdate(entity);
			}
		}

		public override IEnumerable<ValidationResult> ValidateDelete(BudgetClass entity)
		{
			return Enumerable.Empty<ValidationResult>();
		}

		public override IEnumerable<ValidationResult> ValidateSave(BudgetClass entity)
		{
			return Enumerable.Empty<ValidationResult>();
		}

		internal virtual void CommonSaveOperations(BudgetClass entity)
		{
		}
	}
}