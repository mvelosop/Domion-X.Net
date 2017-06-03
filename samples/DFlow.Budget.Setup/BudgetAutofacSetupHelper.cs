using Autofac;
using DFlow.Budget.Lib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DFlow.Budget.Setup
{
    public class BudgetAutofacSetupHelper : Autofac.Module
    {
        private const string _moduleName = "DFlow.Budget";

        private BudgetDbSetupHelper _dbSetupHelper;

        public BudgetAutofacSetupHelper(BudgetDbSetupHelper dbSetupHelper)
        {
            _dbSetupHelper = dbSetupHelper ?? throw new ArgumentNullException(nameof(dbSetupHelper));
        }

        public void SetupContainer(ContainerBuilder builder)
        {
            SetupDependencyResolver(builder);
        }

        protected override void Load(ContainerBuilder builder)
        {
            SetupDependencyResolver(builder);
        }

        private Assembly[] GetModuleAssemblies()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;

            Assembly[] appAssemblies = currentDomain.GetAssemblies()
                .Where(a => a.GetName().Name.StartsWith(_moduleName))
                .ToArray();

            return appAssemblies;
        }

        private void RegisterTypes(ContainerBuilder builder, Assembly[] appAssemblies)
        {
            builder.Register<BudgetDbContext>((c) => _dbSetupHelper.GetDbContext())
                .InstancePerLifetimeScope();

            foreach (var asm in appAssemblies)
            {
                builder.RegisterAssemblyTypes(asm)
                    .Where(t => t.Name.EndsWith("Manager"))
                    .InstancePerLifetimeScope()
                    .AsSelf()
                    .AsImplementedInterfaces();

                builder.RegisterAssemblyTypes(asm)
                    .Where(t => t.Name.EndsWith("ManagerHelper"))
                    .InstancePerLifetimeScope();
            }
        }

        private void SetupDependencyResolver(ContainerBuilder builder)
        {
            Assembly[] appAssemblies = GetModuleAssemblies();

            RegisterTypes(builder, appAssemblies);
        }
    }
}
