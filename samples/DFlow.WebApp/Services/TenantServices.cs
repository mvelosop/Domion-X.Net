using DFlow.Tenants.Core.Model;
using DFlow.Tenants.Lib.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DFlow.WebApp.Services
{
    public class TenantServices
    {
        private readonly Lazy<TenantManager> LazyTenantManager;

        public TenantServices(
            Lazy<TenantManager> lazyTenantManager)
        {
            LazyTenantManager = lazyTenantManager;
        }

        private TenantManager TenantManager => LazyTenantManager.Value;

        public IQueryable<Tenant> Query(Expression<Func<Tenant, bool>> expression = null)
        {
            return TenantManager.Query(expression);
        }
    }
}
