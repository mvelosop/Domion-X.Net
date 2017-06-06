using Autofac;
using DFlow.Transactions.Lib.Data;
using Domion.Setup;
using System;
using System.Linq;
using System.Reflection;

namespace DFlow.Transactions.Setup
{
    public class TransactionsAutofacSetupHelper : Autofac.Module
    {
        private const string _modulePrefix = "DFlow.Transactions";

        private TransactionsDbSetupHelper _dbSetupHelper;

        public TransactionsAutofacSetupHelper(TransactionsDbSetupHelper dbSetupHelper)
        {
            _dbSetupHelper = dbSetupHelper ?? throw new ArgumentNullException(nameof(dbSetupHelper));
        }

        public void SetupContainer(ContainerBuilder builder)
        {
            builder.Register<TransactionsDbContext>((c) => _dbSetupHelper.CreateDbContext())
                .InstancePerLifetimeScope();

            ModuleSetupHelper.RegisterCommonModuleTypes(builder, _modulePrefix);
        }

        protected override void Load(ContainerBuilder builder)
        {
            SetupContainer(builder);
        }
    }
}
