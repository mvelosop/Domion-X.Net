using Autofac;
using DFlow.Tenants.Lib.Data;
using Domion.Setup;
using System;

namespace DFlow.Tenants.Setup
{
    public class TenantsContainerHelper : BaseContainerHelper
    {
        private const string ModulePrefix = "DFlow.Tenants";

        private readonly TenantsDatabaseHelper _dbHelper;

        public TenantsContainerHelper(TenantsDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper ?? throw new ArgumentNullException(nameof(dbHelper));
        }

        /// <summary>
        ///     Adds module's classes registration to the received container builder
        /// </summary>
        /// <param name="builder"></param>
        public void RegisterTypes(ContainerBuilder builder)
        {
            // This defers instance registration until it is actually needed
            builder.Register<TenantsDbContext>((c) => _dbHelper.CreateDbContext())
                .InstancePerLifetimeScope();

            RegisterCommonModuleTypes(builder, ModulePrefix, new[] { "DataHelper", "DataMapper", "Manager", "ManagerHelper", "Repository", "RepositoryHelper" });
        }
    }
}
