using Autofac;
using DFlow.Tenants.Lib.Data;
using DFlow.Tenants.Setup;
using Domion.Setup;
using System;

namespace DFlow.Tenants.Setup
{
    public class TenantsAutofacSetupHelper : Autofac.Module
    {
        private const string _modulePrefix = "DFlow.Tenants";

        private TenantsDbSetupHelper _dbSetupHelper;

        public TenantsAutofacSetupHelper(TenantsDbSetupHelper dbSetupHelper)
        {
            _dbSetupHelper = dbSetupHelper ?? throw new ArgumentNullException(nameof(dbSetupHelper));
        }

        public void SetupContainer(ContainerBuilder builder)
        {
            builder.Register<TenantsDbContext>((c) => _dbSetupHelper.CreateDbContext())
                .InstancePerLifetimeScope();

            ModuleSetupHelper.RegisterCommonModuleTypes(builder, _modulePrefix);
        }

        protected override void Load(ContainerBuilder builder)
        {
            SetupContainer(builder);
        }
    }
}
