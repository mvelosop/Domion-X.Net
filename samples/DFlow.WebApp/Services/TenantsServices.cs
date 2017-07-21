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
        private readonly Lazy<TenantManager> _lazyTenantManager;
        readonly ILogger<TenantsServices> _logger;
        
        private static readonly List<ValidationResult> NoErrors = new List<ValidationResult>();

        public TenantsServices(
            ILogger<TenantsServices> logger,
            Lazy<TenantManager> lazyTenantManager)
        {
            _logger = logger;
            _lazyTenantManager = lazyTenantManager;
        }

        private TenantManager TenantManager => _lazyTenantManager.Value;

        public List<ValidationResult> AddTenant(Tenant entity)
        {
            var errors = TenantManager.TryInsert(entity).ToList();

            if (errors.Any()) return errors;

            TenantManager.SaveChanges();

            return NoErrors;
        }

        public List<ValidationResult> DeleteTenant(Tenant entity)
        {
            var errors = TenantManager.TryDelete(entity).ToList();

            if (errors.Any()) return errors;

            TenantManager.SaveChanges();

            return NoErrors;
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

        public IQueryable<Tenant> Search(string searchText)
        {

            Expression<Func<Tenant, bool>> queryExpression = null;

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                queryExpression = t => t.Owner.Contains(searchText);
            }
                
            return TenantManager.Query(queryExpression);
        }

        public List<ValidationResult> UpdateTenant(Tenant entity)
        {
            var errors = TenantManager.TryUpdate(entity).ToList();

            if (errors.Any()) return errors;

            TenantManager.SaveChanges();

            return NoErrors;
        }
        public List<ValidationResult> ValidateDelete(Tenant entity)
        {
            var errors = TenantManager.ValidateDelete(entity).ToList();

            if (errors.Any()) return errors;

            TenantManager.SaveChanges();

            return NoErrors;
        }
    }
}
