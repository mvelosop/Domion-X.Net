using Autofac;
using DFlow.Budget.Lib.Data;
using Domion.Setup;
using System;

namespace DFlow.Budget.Setup
{
    public class BudgetContainerSetup : BaseContainerSetup
    {
        private const string _modulePrefix = "DFlow.Budget";

        private readonly BudgetDbHelper DbHelper;

        public BudgetContainerSetup(BudgetDbHelper dbHelper)
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
            builder.Register<BudgetDbContext>((c) => DbHelper.CreateDbContext())
                .InstancePerLifetimeScope();

            RegisterCommonModuleTypes(builder, _modulePrefix);
        }
    }
}
