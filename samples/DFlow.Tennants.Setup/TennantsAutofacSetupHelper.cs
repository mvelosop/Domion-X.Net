using Autofac;
using DFlow.Tennants.Lib.Data;
using Domion.Setup;
using System;

namespace DFlow.Tennants.Setup
{
    public class TennantsAutofacSetupHelper : Autofac.Module
    {
        private const string _modulePrefix = "DFlow.Tennants";

        private TennantsDbSetupHelper _dbSetupHelper;

        public TennantsAutofacSetupHelper(TennantsDbSetupHelper dbSetupHelper)
        {
            _dbSetupHelper = dbSetupHelper ?? throw new ArgumentNullException(nameof(dbSetupHelper));
        }

        public void SetupContainer(ContainerBuilder builder)
        {
            builder.Register<TennantsDbContext>((c) => _dbSetupHelper.CreateDbContext())
                .InstancePerLifetimeScope();

            ModuleSetupHelper.RegisterCommonModuleTypes(builder, _modulePrefix);
        }

        protected override void Load(ContainerBuilder builder)
        {
            SetupContainer(builder);
        }
    }
}
