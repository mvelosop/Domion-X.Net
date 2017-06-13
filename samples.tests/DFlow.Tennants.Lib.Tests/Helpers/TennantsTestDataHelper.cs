using Autofac;

namespace DFlow.Tennants.Lib.Tests.Helpers
{
    public class TennantsTestDataHelper
    {
        ILifetimeScope _scope;

        public TennantsTestDataHelper(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public void SeedReferenceTennants()
        {
            var managerHelper = _scope.Resolve<TennantManagerHelper>();

            var dataSet = new TennantData[]
            {
                new TennantData("Reference Tennant #1"),
                new TennantData("Reference Tennant #2"),
                new TennantData("Reference Tennant #3"),
            };

            managerHelper.EnsureEntitiesExist(dataSet);
        }
    }
}
