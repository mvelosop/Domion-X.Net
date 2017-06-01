//------------------------------------------------------------------------------
// CashTransactionManager.cs
//
// Implementation of: CashTransactionManager (Class) <<entity-manager>>
// Generated with Domion-MDA - http://www.coderepo.blog/domion
//------------------------------------------------------------------------------

using Demo.Transactions.Core.Model;
using Demo.Transactions.Core.Services;
using Demo.Transactions.Lib.Data;
using Domion.Core.Services;
using Domion.Lib.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace Demo.Transactions.Lib.Services
{
    public class CashTransactionManager : BaseRepository<CashTransaction, int>, IEntityManager<CashTransaction, int>, ICashTransactionManager
    {
        public CashTransactionManager(TransactionsDbContext dbContext)
            : base(dbContext)
        {
        }

        public override IQueryable<CashTransaction> Query(Expression<Func<CashTransaction, bool>> where)
        {
            return base.Query(where);
        }

        public virtual CashTransaction Refresh(CashTransaction entity)
        {
            base.Detach(entity);

            return Find(entity.Id);
        }

        public new virtual IEnumerable<ValidationResult> TryDelete(CashTransaction entity)
        {
            return base.TryDelete(entity);
        }

        public new virtual IEnumerable<ValidationResult> TryInsert(CashTransaction entity)
        {
            if (entity.RowVersion != null && entity.RowVersion.Length > 0) throw new InvalidOperationException("RowVersion not empty on Insert");

            CommonSaveOperations(entity);

            return base.TryInsert(entity);
        }

        public new virtual IEnumerable<ValidationResult> TryUpdate(CashTransaction entity)
        {
            if (entity.RowVersion == null || entity.RowVersion.Length == 0) throw new InvalidOperationException("RowVersion empty on Update");

            CommonSaveOperations(entity);

            return base.TryUpdate(entity);
        }

        public virtual IEnumerable<ValidationResult> TryUpsert(CashTransaction entity)
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

        public override IEnumerable<ValidationResult> ValidateDelete(CashTransaction entity)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override IEnumerable<ValidationResult> ValidateSave(CashTransaction entity)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        internal virtual void CommonSaveOperations(CashTransaction entity)
        {
        }
    }
}
