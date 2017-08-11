//------------------------------------------------------------------------------
//  TenantRepository.cs
//
//  Implementation of: TenantRepository (Class) <<entity-repository>>
//  Generated by Domion-MDA - http://www.coderepo.blog/domion
//
//  Created on     : 04-jul-2017 23:57:10
//  Original author: Miguel
//------------------------------------------------------------------------------

using DFlow.Tenants.Core.Model;
using DFlow.Tenants.Core.Services;
using DFlow.Tenants.Lib.Data;
using Domion.Lib.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DFlow.Tenants.Lib.Services
{
    public class TenantRepository : BaseRepository<Tenant, int>, ITenantRepository
    {
        public static readonly string ConcurrentUpdateError = @"The Tenant was updated by another user, can't update or delete, must refresh first! (Id={0})";

        public static readonly string DuplicateByOwnerError = @"There's another Tenant with Owner ""{0}"", can't duplicate! (Id={1})";

        /// <inheritdoc />
        public TenantRepository(TenantsDbContext dbContext)
            : base(dbContext)
        {
        }

        /// <inheritdoc />
        public Tenant FindDuplicateByOwner(Tenant entity)
        {
            Expression<Func<Tenant, bool>> where = t => t.Owner == entity.Owner.Trim();

            IQueryable<Tenant> query = Query().Where(where);

            if (entity.Id != 0)
            {
                query = query.Where(t => t.Id != entity.Id);
            }

            return query.SingleOrDefault();
        }

        /// <summary>
        ///     Returns an IQueryable that, when enumerated, will retrieve the objects that satisfy the where condition
        ///     or all of them if where condition is null.
        /// </summary>
        public override IQueryable<Tenant> Query(Expression<Func<Tenant, bool>> where)
        {
            return base.Query(where);
        }

        /// <inheritdoc />
        public virtual Tenant Refresh(Tenant entity)
        {
            DetachInternal(entity);

            return Find(entity.Id);
        }

        /// <inheritdoc />
        public virtual async Task<Tenant> RefreshAsync(Tenant entity)
        {
            DetachInternal(entity);

            return await FindAsync(entity.Id);
        }

        /// <inheritdoc />
        public new virtual List<ValidationResult> TryDelete(Tenant entity)
        {
            if (entity.RowVersion == null || entity.RowVersion.Length == 0) throw new InvalidOperationException($"Missing {nameof(entity.RowVersion)} on Delete");

            List<ValidationResult> errors = ValidateConcurrentUpdate(entity);

            return errors.Any() ? errors : base.TryDelete(entity);
        }

        /// <inheritdoc />
        public new virtual List<ValidationResult> TryInsert(Tenant entity)
        {
            if (entity.RowVersion != null && entity.RowVersion.Length > 0) throw new InvalidOperationException($"Existing {nameof(entity.RowVersion)} on Insert");

            CommonSaveOperations(entity);

            return base.TryInsert(entity);
        }

        /// <inheritdoc />
        public new virtual List<ValidationResult> TryUpdate(Tenant entity)
        {
            if (entity.RowVersion == null || entity.RowVersion.Length == 0) throw new InvalidOperationException($"Missing {nameof(entity.RowVersion)} on Update");

            List<ValidationResult> errors = ValidateConcurrentUpdate(entity);

            if (errors.Any())
            {
                return errors;
            }

            CommonSaveOperations(entity);

            return base.TryUpdate(entity);
        }

        /// <inheritdoc />
        public virtual List<ValidationResult> TryUpsert(Tenant entity)
        {
            return entity.Id == 0 ? TryInsert(entity) : TryUpdate(entity);
        }

        /// <inheritdoc />
        public override IEnumerable<ValidationResult> ValidateDelete(Tenant entity)
        {
            yield break;
        }

        /// <inheritdoc />
        public override IEnumerable<ValidationResult> ValidateSave(Tenant entity)
        {
            Tenant duplicateByOwner = FindDuplicateByOwner(entity);

            if (duplicateByOwner != null)
            {
                yield return new ValidationResult(string.Format(DuplicateByOwnerError, duplicateByOwner.Owner, duplicateByOwner.Id), new[] { "Owner" });
            }

            yield break;
        }

        /// <summary>
        ///     Performs operations that have to be executed both on inserts and updates.
        /// </summary>
        internal virtual void CommonSaveOperations(Tenant entity)
        {
            TrimStrings(entity);
        }

        private void DetachInternal(Tenant entity)
        {
            Detach(entity);
        }

        private void TrimStrings(Tenant entity)
        {
            if (entity.Owner != null) entity.Owner = entity.Owner.Trim();
        }

        private List<ValidationResult> ValidateConcurrentUpdate(Tenant entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            EntityEntry<Tenant> entry = DbContext.Entry(entity);

            var error = new List<ValidationResult>();

            var originalRowVersion = entry.OriginalValues["RowVersion"] as byte[];

            if (originalRowVersion == null || originalRowVersion.Length == 0)
            {
                throw new InvalidOperationException($"Invalid {nameof(entity.RowVersion)} on update or delete");
            }

            if (!entity.RowVersion.SequenceEqual(originalRowVersion))
            {
                error.Add(new ValidationResult(string.Format(ConcurrentUpdateError, entity.Id)));
            }

            return error;
        }
    }
}
