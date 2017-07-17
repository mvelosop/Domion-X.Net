using DFlow.Tenants.Core.Model;
using DFlow.Tenants.Lib.Services;
using Domion.Lib.Extensions;
using Microsoft.Extensions.Logging;
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
        readonly ILogger<TenantsServices> Logger;

        public TenantsServices(
            ILogger<TenantsServices> logger,
            Lazy<TenantManager> lazyTenantManager)
        {
            Logger = logger;
            LazyTenantManager = lazyTenantManager;
        }

        private TenantManager TenantManager => LazyTenantManager.Value;

        public IEnumerable<ValidationResult> AddTenant(Tenant entity)
        {
            var errors = TenantManager.TryInsert(entity).ToList();

            if (errors.Any()) return errors;

            TenantManager.SaveChanges();

            return Enumerable.Empty<ValidationResult>();
        }

        public IEnumerable<ValidationResult> DeleteTenant(Tenant entity)
        {
            var errors = TenantManager.TryDelete(entity).ToList();

            if (errors.Any()) return errors;

            TenantManager.SaveChanges();

            return Enumerable.Empty<ValidationResult>();
        }

        public Tenant FindTenantById(int? id)
        {
            if (id == null)
            {
                return null;
            }

            Tenant entity = TenantManager.SingleOrDefault(t => t.Id == id);

            return entity;
        }

        public IQueryable<Tenant> Query(Expression<Func<Tenant, bool>> expression = null)
        {
            return TenantManager.Query(expression);
        }

        public IEnumerable<ValidationResult> UpdateTenant(Tenant entity)
        {
            var errors = TenantManager.TryUpdate(entity).ToList();

            if (errors.Any()) return errors;

            TenantManager.SaveChanges();

            return Enumerable.Empty<ValidationResult>();
        }
        public IEnumerable<ValidationResult> ValidateDelete(Tenant entity)
        {
            if (entity.Owner.ToUpper().Contains("NO ELIMINAR"))
            {
                return new List<ValidationResult>
                {
                    new ValidationResult(@"No se puede eliminar porque dice ""NO ELIMINAR"".")
                };
            }

            var errors = TenantManager.ValidateDelete(entity).ToList();

            if (errors.Any()) return errors;

            TenantManager.SaveChanges();

            return Enumerable.Empty<ValidationResult>();
        }
    }
}
