﻿using Autofac;
using DFlow.Tennants.Lib.Data;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DFlow.Tennants.Setup
{
    public class TennantsAutofacSetupHelper : Autofac.Module
    {
        private const string _moduleName = "DFlow.Tennants";

        private TennantsDbSetupHelper _dbSetupHelper;

        public TennantsAutofacSetupHelper(TennantsDbSetupHelper dbSetupHelper)
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

        private Assembly[] GetModuleAssemblies(string moduleName)
        {
            // From: https://stackoverflow.com/questions/2384592/is-there-a-way-to-force-all-referenced-assemblies-to-be-loaded-into-the-app-doma
            // With some fixes!

            AppDomain currentDomain = AppDomain.CurrentDomain;

            var loadedAssemblies = currentDomain.GetAssemblies().Where(a => a.FullName.StartsWith(moduleName)).ToList();

            var loadedPaths = loadedAssemblies.Select(a => a.CodeBase.Replace("file:///", "").Replace('/', Path.DirectorySeparatorChar)).ToArray();

            var referencedPaths = Directory.GetFiles(currentDomain.BaseDirectory, $"{moduleName}*.dll");

            var toLoad = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();

            toLoad.ForEach(path => loadedAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path))));

            return loadedAssemblies.ToArray();
        }

        private void RegisterTypes(ContainerBuilder builder, Assembly[] appAssemblies)
        {
            builder.Register<TennantsDbContext>((c) => _dbSetupHelper.GetDbContext())
                .InstancePerLifetimeScope();

            foreach (var asm in appAssemblies)
            {
                builder.RegisterAssemblyTypes(asm)
                    .Where(t => t.Name.EndsWith("DataHelper"))
                    .InstancePerLifetimeScope();

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
            Assembly[] appAssemblies = GetModuleAssemblies(_moduleName);

            RegisterTypes(builder, appAssemblies);
        }
    }
}