using Autofac;

namespace DFlow.Tenants.Lib.Tests.Helpers
{
    public class TenantsTestDataHelper
    {
        ILifetimeScope _scope;

        public TenantsTestDataHelper(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public void SeedReferenceTenants()
        {
            var managerHelper = _scope.Resolve<TenantManagerHelper>();

            var dataSet = new TenantData[]
            {
                new TenantData("Reference Tenant #1"),
                new TenantData("Reference Tenant #2"),
                new TenantData("Reference Tenant #3"),
            };

            managerHelper.EnsureEntitiesExist(dataSet);
        }
    }
}
