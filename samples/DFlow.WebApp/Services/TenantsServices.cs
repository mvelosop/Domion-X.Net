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
        private readonly Lazy<TenantRepository> _lazyTenantRepo;
        readonly ILogger<TenantsServices> _logger;
        
        private static readonly List<ValidationResult> NoErrors = new List<ValidationResult>();

        public TenantsServices(
            ILogger<TenantsServices> logger,
            Lazy<TenantRepository> lazyTenantRepo)
        {
            _logger = logger;
            _lazyTenantRepo = lazyTenantRepo;
        }

        private TenantRepository TenantRepo => _lazyTenantRepo.Value;

        public async Task<List<ValidationResult>> AddTenant(Tenant entity)
        {
            List<ValidationResult> errors = TenantRepo.TryInsert(entity).ToList();

            if (errors.Any()) return errors;

            TenantRepo.SaveChanges();

            return NoErrors;
        }

        public async Task<List<ValidationResult>> DeleteTenant(Tenant entity)
        {
            List<ValidationResult> errors = TenantRepo.TryDelete(entity).ToList();

            if (errors.Any()) return errors;

            TenantRepo.SaveChanges();

            return NoErrors;
        }

        public async Task<Tenant> FindTenantById(int? id)
        {
            if (id == null)
            {
                return null;
            }

            Tenant entity = TenantRepo.SingleOrDefault(t => t.Id == id);

            return entity;
        }

        public async Task<IQueryable<Tenant>> Search(string searchText)
        {

            Expression<Func<Tenant, bool>> queryExpression = null;

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                queryExpression = t => t.Owner.Contains(searchText);
            }
                
            return TenantRepo.Query(queryExpression);
        }

        public async Task<List<ValidationResult>> UpdateTenant(Tenant entity)
        {
            List<ValidationResult> errors = TenantRepo.TryUpdate(entity).ToList();

            if (errors.Any()) return errors;

            TenantRepo.SaveChanges();

            return NoErrors;
        }
        public async Task<List<ValidationResult>> ValidateDelete(Tenant entity)
        {
            List<ValidationResult> errors = TenantRepo.ValidateDelete(entity).ToList();

            if (errors.Any()) return errors;

            TenantRepo.SaveChanges();

            return NoErrors;
        }
    }
}
