using DFlow.Tenants.Core.Model;
using DFlow.Tenants.Lib.Services;
using Domion.Lib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace DFlow.WebApp.Services
{
    public class TenantsServices
    {
        private readonly Lazy<TenantManager> LazyTenantManager;

        public TenantsServices(
            Lazy<TenantManager> lazyTenantManager)
        {
            LazyTenantManager = lazyTenantManager;
        }

        private TenantManager TenantManager => LazyTenantManager.Value;

        public IEnumerable<ValidationResult> AddTenant(Tenant entity)
        {
            var errors = TenantManager.TryInsert(entity).ToList();

            if (errors.Count > 0) return errors;

            TenantManager.SaveChanges();

            return Enumerable.Empty<ValidationResult>();
        }

        public IEnumerable<ValidationResult> DeleteTenant(Tenant entity)
        {
            var errors = TenantManager.TryDelete(entity).ToList();

            if (errors.Count > 0) return errors;

            TenantManager.SaveChanges();

            return Enumerable.Empty<ValidationResult>();
        }

        public Tenant FindTenantById(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return TenantManager.SingleOrDefault(t => t.Id == id);
        }

        public IQueryable<Tenant> Query(Expression<Func<Tenant, bool>> expression = null)
        {
            return TenantManager.Query(expression);
        }

        public IEnumerable<ValidationResult> UpdateTenant(Tenant entity)
        {
            var errors = TenantManager.TryUpdate(entity).ToList();

            if (errors.Count > 0) return errors;

            TenantManager.SaveChanges();

            return Enumerable.Empty<ValidationResult>();
        }
        public IEnumerable<ValidationResult> ValidateDelete(Tenant entity)
        {
            var errors = TenantManager.ValidateDelete(entity).ToList();

            if (errors.Count > 0) return errors;

            TenantManager.SaveChanges();

            return Enumerable.Empty<ValidationResult>();
        }
    }
}
