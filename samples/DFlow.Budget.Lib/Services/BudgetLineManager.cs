//------------------------------------------------------------------------------
//  BudgetLineManager.cs
//
//  Implementation of: BudgetLineManager (Class) <<entity-manager>>
//  Generated by Domion-MDA - http://www.coderepo.blog/domion
//
//  Created on     : 02-jun-2017 10:49:08
//  Original author: Miguel
//------------------------------------------------------------------------------

using DFlow.Budget.Core.Model;
using DFlow.Budget.Core.Services;
using DFlow.Budget.Lib.Data;
using Domion.Core.Services;
using Domion.Lib.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace DFlow.Budget.Lib.Services
{
    public class BudgetLineManager : BaseRepository<BudgetLine, int>, IQueryManager<BudgetLine>, IEntityManager<BudgetLine, int>, IBudgetLineManager
    {
        public static string duplicateByNameError = @"There's another BudgetLine with Name ""{0}"", can't duplicate (Id={1})!";

        public BudgetLineManager(BudgetDbContext dbContext)
            : base(dbContext)
        {
        }

        public BudgetLine FindDuplicateByName(BudgetLine entity)
        {
            if (entity.Id == 0)
            {
                return Query(bl => bl.Name == entity.Name.Trim()).SingleOrDefault();
            }
            else
            {
                return Query(bl => bl.Name == entity.Name.Trim() && bl.Id != entity.Id).SingleOrDefault();
            }
        }

        public override IQueryable<BudgetLine> Query(Expression<Func<BudgetLine, bool>> where)
        {
            return base.Query(where);
        }

        public virtual BudgetLine Refresh(BudgetLine entity)
        {
            base.Detach(entity);

            return Find(entity.Id);
        }

        public new virtual IEnumerable<ValidationResult> TryDelete(BudgetLine entity)
        {
            return base.TryDelete(entity);
        }

        public new virtual IEnumerable<ValidationResult> TryInsert(BudgetLine entity)
        {
            if (entity.RowVersion != null && entity.RowVersion.Length > 0) throw new InvalidOperationException("RowVersion not empty on Insert");

            CommonSaveOperations(entity);

            return base.TryInsert(entity);
        }

        public new virtual IEnumerable<ValidationResult> TryUpdate(BudgetLine entity)
        {
            if (entity.RowVersion == null || entity.RowVersion.Length == 0) throw new InvalidOperationException("RowVersion empty on Update");

            CommonSaveOperations(entity);

            return base.TryUpdate(entity);
        }

        public virtual IEnumerable<ValidationResult> TryUpsert(BudgetLine entity)
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

        internal virtual void CommonSaveOperations(BudgetLine entity)
        {
            TrimStrings(entity);
        }

        protected override IEnumerable<ValidationResult> ValidateDelete(BudgetLine entity)
        {
            yield break;
        }

        protected override IEnumerable<ValidationResult> ValidateSave(BudgetLine entity)
        {
            BudgetLine duplicateByName = FindDuplicateByName(entity);

            if (duplicateByName != null)
            {
                yield return new ValidationResult(string.Format(duplicateByNameError, duplicateByName.Name, duplicateByName.Id), new[] { "Name" });
            }

            yield break;
        }

        private void TrimStrings(BudgetLine entity)
        {
            if (entity.Name != null) entity.Name = entity.Name.Trim();
        }
    }
}
