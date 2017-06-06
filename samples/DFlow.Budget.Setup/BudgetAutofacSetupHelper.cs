using Autofac;
using DFlow.Budget.Lib.Data;
using Domion.Setup;
using System;

namespace DFlow.Budget.Setup
{
    public class BudgetAutofacSetupHelper : Autofac.Module
    {
        private const string _modulePrefix = "DFlow.Budget";

        private BudgetDbSetupHelper _dbSetupHelper;

        public BudgetAutofacSetupHelper(BudgetDbSetupHelper dbSetupHelper)
        {
            _dbSetupHelper = dbSetupHelper ?? throw new ArgumentNullException(nameof(dbSetupHelper));
        }

        public void SetupContainer(ContainerBuilder builder)
        {
            builder.Register<BudgetDbContext>((c) => _dbSetupHelper.CreateDbContext())
                .InstancePerLifetimeScope();

            ModuleSetupHelper.RegisterCommonModuleTypes(builder, _modulePrefix);
        }

        protected override void Load(ContainerBuilder builder)
        {
            SetupContainer(builder);
        }
    }
}
