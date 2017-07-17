//------------------------------------------------------------------------------
//  TenantManager.cs
//
//  Implementation of: TenantManager (Class) <<entity-manager>>
//  Generated by Domion-MDA - http://www.coderepo.blog/domion
//
//  Created on     : 04-jul-2017 23:57:10
//  Original author: Miguel
//------------------------------------------------------------------------------

using DFlow.Tenants.Core.Model;
using DFlow.Tenants.Core.Services;
using DFlow.Tenants.Lib.Data;
using Domion.Core.Services;
using Domion.Lib.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace DFlow.Tenants.Lib.Services
{
    public class TenantManager : BaseRepository<Tenant, int>, IQueryManager<Tenant>, IEntityManager<Tenant, int>, ITenantManager
    {
        public static readonly string ConcurrentUpdateError = @"The Tenant was updated by another user, can't update or delete, must refresh first! (Id={0})";

        public static readonly string DuplicateByOwnerError = @"There's another Tenant with Owner ""{0}"", can't duplicate! (Id={1})";

        public TenantManager(TenantsDbContext dbContext)
            : base(dbContext)
        {
        }

        public Tenant FindDuplicateByOwner(Tenant entity)
        {
            if (entity.Id == 0)
            {
                return Query(t => t.Owner == entity.Owner.Trim()).SingleOrDefault();
            }
            else
            {
                return Query(t => t.Owner == entity.Owner.Trim() && t.Id != entity.Id).SingleOrDefault();
            }
        }

        public override IQueryable<Tenant> Query(Expression<Func<Tenant, bool>> where)
        {
            return base.Query(where);
        }

        public virtual Tenant Refresh(Tenant entity)
        {
            base.Detach(entity);

            return Find(entity.Id);
        }

        public new virtual IEnumerable<ValidationResult> TryDelete(Tenant entity)
        {
            if (entity.RowVersion == null || entity.RowVersion.Length == 0) throw new InvalidOperationException("RowVersion empty on Delete");

            var error = ValidateConcurrentUpdate(entity);

            if (error.Any())
            {
                return error;
            }

            return base.TryDelete(entity);
        }

        public new virtual IEnumerable<ValidationResult> TryInsert(Tenant entity)
        {
            if (entity.RowVersion != null && entity.RowVersion.Length > 0) throw new InvalidOperationException("RowVersion not empty on Insert");

            CommonSaveOperations(entity);

            return base.TryInsert(entity);
        }

        public new virtual IEnumerable<ValidationResult> TryUpdate(Tenant entity)
        {
            if (entity.RowVersion == null || entity.RowVersion.Length == 0) throw new InvalidOperationException("RowVersion empty on Update");

            var error = ValidateConcurrentUpdate(entity);

            if (error.Any())
            {
                return error;
            }

            CommonSaveOperations(entity);

            return base.TryUpdate(entity);
        }

        public virtual IEnumerable<ValidationResult> TryUpsert(Tenant entity)
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

        public override IEnumerable<ValidationResult> ValidateDelete(Tenant entity)
        {
            yield break;
        }

        public override IEnumerable<ValidationResult> ValidateSave(Tenant entity)
        {
            Tenant duplicateByOwner = FindDuplicateByOwner(entity);

            if (duplicateByOwner != null)
            {
                yield return new ValidationResult(string.Format(DuplicateByOwnerError, duplicateByOwner.Owner, duplicateByOwner.Id), new[] { "Owner" });
            }

            yield break;
        }

        internal virtual void CommonSaveOperations(Tenant entity)
        {
            TrimStrings(entity);
        }

        private void TrimStrings(Tenant entity)
        {
            if (entity.Owner != null) entity.Owner = entity.Owner.Trim();
        }

        private List<ValidationResult> ValidateConcurrentUpdate(Tenant entity)
        {
            EntityEntry<Tenant> entry = DbContext.Entry(entity);

            var error = new List<ValidationResult>();

            if (!entity.RowVersion.SequenceEqual(entry.OriginalValues["RowVersion"] as byte[]))
            {
                error.Add(new ValidationResult(string.Format(ConcurrentUpdateError, entity.Id)));
            };

            return error;
        }
    }
}
