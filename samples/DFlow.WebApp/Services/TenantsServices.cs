using DFlow.Tenants.Core.Model;
using DFlow.Tenants.Lib.Services;
using Domion.Lib.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

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

        public async Task<List<ValidationResult>> AddTenant(Tenant entity)
        {
            List<ValidationResult> errors = TenantManager.TryInsert(entity).ToList();

            if (errors.Any()) return errors;

            TenantManager.SaveChanges();

            return NoErrors;
        }

        public async Task<List<ValidationResult>> DeleteTenant(Tenant entity)
        {
            List<ValidationResult> errors = TenantManager.TryDelete(entity).ToList();

            if (errors.Any()) return errors;

            TenantManager.SaveChanges();

            return NoErrors;
        }

        public async Task<Tenant> FindTenantById(int? id)
        {
            if (id == null)
            {
                return null;
            }

            Tenant entity = TenantManager.SingleOrDefault(t => t.Id == id);

            return entity;
        }

        public async Task<IQueryable<Tenant>> Search(string searchText)
        {

            Expression<Func<Tenant, bool>> queryExpression = null;

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                queryExpression = t => t.Owner.Contains(searchText);
            }
                
            return TenantManager.Query(queryExpression);
        }

        public async Task<List<ValidationResult>> UpdateTenant(Tenant entity)
        {
            List<ValidationResult> errors = TenantManager.TryUpdate(entity).ToList();

            if (errors.Any()) return errors;

            TenantManager.SaveChanges();

            return NoErrors;
        }
        public async Task<List<ValidationResult>> ValidateDelete(Tenant entity)
        {
            List<ValidationResult> errors = TenantManager.ValidateDelete(entity).ToList();

            if (errors.Any()) return errors;

            TenantManager.SaveChanges();

            return NoErrors;
        }
    }
}
