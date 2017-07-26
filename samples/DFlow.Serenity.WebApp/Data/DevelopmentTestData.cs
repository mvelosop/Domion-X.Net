using Autofac;
using DFlow.Tenants.Lib.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFlow.Serenity.WebApp.Data
{
    public class DevelopmentTestData
    {
        readonly ILifetimeScope Scope;

        public DevelopmentTestData(ILifetimeScope scope)
        {
            Scope = scope;
        }

        public void Load()
        {
            var tenantDataSet = new TenantData[]
            {
                new TenantData("Tenant 01"),
                new TenantData("Tenant 02"),
                new TenantData("Tenant 03"),
                new TenantData("Tenant 04"),
                new TenantData("Tenant 05"),
                new TenantData("Tenant 06"),
                new TenantData("Tenant 07"),
                new TenantData("Tenant 08"),
                new TenantData("Tenant 09"),
                new TenantData("Tenant 10"),
                new TenantData("Tenant 11"),
                new TenantData("Tenant 12"),
            };

            using (var scope = Scope.BeginLifetimeScope())
            {
                var helper = scope.Resolve<TenantRepositoryHelper>();

                helper.EnsureEntitiesExist(tenantDataSet);
            }
        }
    }
}
