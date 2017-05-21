//------------------------------------------------------------------------------
// TagManager.cs
//
// Implementation of: TagManager (Class) <<entity-manager>>
// Generated with Domion-MDA - www.coderepo.blog/domion
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
    public class TagManager : BaseRepository<Tag, int>, IEntityManager<Tag, int>, ITagManager
    {
        public static string duplicateByNameError = @"Ya existe un tag con nombre ""{0}"", no se puede duplicar";

        public TagManager(TransactionsDbContext dbContext)
            : base(dbContext)
        {
        }

        public override IQueryable<Tag> Query(Expression<Func<Tag, bool>> where)
        {
            return base.Query(where);
        }

        public virtual Tag Refresh(Tag entity)
        {
            base.Detach(entity);

            return Find(entity.Id);
        }

        public new virtual IEnumerable<ValidationResult> TryDelete(Tag entity)
        {
            return base.TryDelete(entity);
        }

        public new virtual IEnumerable<ValidationResult> TryInsert(Tag entity)
        {
            if (entity.RowVersion != null && entity.RowVersion.Length > 0) throw new InvalidOperationException("RowVersion not empty on Insert");

            CommonSaveOperations(entity);

            return base.TryInsert(entity);
        }

        public new virtual IEnumerable<ValidationResult> TryUpdate(Tag entity)
        {
            if (entity.RowVersion == null || entity.RowVersion.Length == 0) throw new InvalidOperationException("RowVersion empty on Update");

            CommonSaveOperations(entity);

            return base.TryUpdate(entity);
        }

        public virtual IEnumerable<ValidationResult> TryUpsert(Tag entity)
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

        public override IEnumerable<ValidationResult> ValidateDelete(Tag entity)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override IEnumerable<ValidationResult> ValidateSave(Tag entity)
        {
            var duplicated = FindDuplicateByName(entity);

            if (duplicated != null)
            {
                yield return new ValidationResult(String.Format(duplicateByNameError, duplicated.Name), new[] { "Name" });
            }
        }

        internal virtual void CommonSaveOperations(Tag entity)
        {
        }

        private Tag FindDuplicateByName(Tag entity)
        {
            Expression<Func<Tag, bool>> expression = null;

            if (entity.Id == 0)
            {
                return SingleOrDefault(bc => bc.Name == entity.Name);
            }
            else
            {
                return SingleOrDefault(bc => bc.Name == entity.Name && bc.Id != entity.Id);
            }
        }

    }
}