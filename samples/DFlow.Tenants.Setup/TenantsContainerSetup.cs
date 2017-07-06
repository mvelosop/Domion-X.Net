using Autofac;
using DFlow.Tenants.Lib.Data;
using Domion.Setup;
using System;

namespace DFlow.Tenants.Setup
{
    public class TenantsContainerSetup : BaseContainerSetup
    {
        private const string _modulePrefix = "DFlow.Tenants";

        private readonly TenantsDbHelper DbHelper;

        public TenantsContainerSetup(TenantsDbHelper dbHelper)
        {
            DbHelper = dbHelper ?? throw new ArgumentNullException(nameof(dbHelper));
        }

        /// <summary>
        ///     Adds module's classes registration to the received container builder
        /// </summary>
        /// <param name="builder"></param>
        public void RegisterTypes(ContainerBuilder builder)
        {
            // This defers instance registration until it is actually needed
            builder.Register<TenantsDbContext>((c) => DbHelper.CreateDbContext())
                .InstancePerLifetimeScope();

            RegisterCommonModuleTypes(builder, _modulePrefix);
        }
    }
}
