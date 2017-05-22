//------------------------------------------------------------------------------
// BankAccountManager.cs
//
// Implementation of: BankAccountManager (Class) <<entity-manager>>
// Generated with Domion-MDA - www.coderepo.blog
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
    public class BankAccountManager : BaseRepository<BankAccount, int>, IEntityManager<BankAccount, int>, IBankAccountManager
    {
        public static string duplicateByAccountNumberError = @"Ya existe una cuenta con código ""{0}"", no se puede duplicar";

        public BankAccountManager(TransactionsDbContext dbContext)
            : base(dbContext)
        {
        }

        public override IQueryable<BankAccount> Query(Expression<Func<BankAccount, bool>> where)
        {
            return base.Query(where);
        }

        public virtual BankAccount Refresh(BankAccount entity)
        {
            base.Detach(entity);

            return Find(entity.Id);
        }

        public new virtual IEnumerable<ValidationResult> TryDelete(BankAccount entity)
        {
            return base.TryDelete(entity);
        }

        public new virtual IEnumerable<ValidationResult> TryInsert(BankAccount entity)
        {
            if (entity.RowVersion != null && entity.RowVersion.Length > 0) throw new InvalidOperationException("RowVersion not empty on Insert");

            CommonSaveOperations(entity);

            return base.TryInsert(entity);
        }

        public new virtual IEnumerable<ValidationResult> TryUpdate(BankAccount entity)
        {
            if (entity.RowVersion == null || entity.RowVersion.Length == 0) throw new InvalidOperationException("RowVersion empty on Update");

            CommonSaveOperations(entity);

            return base.TryUpdate(entity);
        }

        public virtual IEnumerable<ValidationResult> TryUpsert(BankAccount entity)
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

        public override IEnumerable<ValidationResult> ValidateDelete(BankAccount entity)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override IEnumerable<ValidationResult> ValidateSave(BankAccount entity)
        {
            BankAccount duplicate = FindDuplicate(entity);

            if (duplicate != null)
            {
                yield return new ValidationResult(String.Format(duplicateByAccountNumberError, duplicate.AccountNumber),
                    new[] { nameof(BankAccount.AccountNumber) });
            }
        }

        internal virtual void CommonSaveOperations(BankAccount entity)
        {
        }

        private BankAccount FindDuplicate(BankAccount entity)
        {
            if (entity.Id == 0)
            {
                return SingleOrDefault(ba => ba.AccountNumber == entity.AccountNumber);
            }
            else
            {
                return SingleOrDefault(ba => ba.AccountNumber == entity.AccountNumber && ba.Id != entity.Id);
            }
        }
    }
}
