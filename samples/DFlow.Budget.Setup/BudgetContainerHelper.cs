using Autofac;
using DFlow.Budget.Lib.Data;
using DFlow.Tenants.Setup;
using Domion.Setup;
using System;

namespace DFlow.Budget.Setup
{
    public class BudgetContainerHelper : BaseContainerHelper
    {
        private const string ModulePrefix = "DFlow.Budget";

        private readonly BudgetDatabaseHelper _dbHelper;

        public BudgetContainerHelper(BudgetDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper ?? throw new ArgumentNullException(nameof(dbHelper));
        }

        /// <summary>
        ///     Adds module's classes registration to the received container builder
        /// </summary>
        /// <param name="builder"></param>
        public void RegisterTypes(ContainerBuilder builder)
        {
            RegisterExternalTypes(builder);

            // This defers instance registration until it is actually needed
            builder.Register<BudgetDbContext>((c) => _dbHelper.CreateDbContext())
                .InstancePerLifetimeScope();

            RegisterCommonModuleTypes(builder, ModulePrefix, new []{ "DataHelper", "DataMapper", "Manager", "ManagerHelper", "Repository", "RepositoryHelper" });
        }

        private void RegisterExternalTypes(ContainerBuilder builder)
        {
            var containerHelper = new TenantsContainerHelper(_dbHelper.TenantsDbHelper);

            containerHelper.RegisterTypes(builder);
        }
    }
}
