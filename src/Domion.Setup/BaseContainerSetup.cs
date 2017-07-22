using Autofac;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Domion.Setup
{
    public class BaseContainerSetup
    {
        protected static Assembly[] LoadModuleAssemblies(string modulePrefix)
        {
            // From: https://stackoverflow.com/questions/2384592/is-there-a-way-to-force-all-referenced-assemblies-to-be-loaded-into-the-app-doma
            // With some fixes!

            AppDomain currentDomain = AppDomain.CurrentDomain;

            var loadedAssemblies = currentDomain.GetAssemblies().Where(a => a.FullName.StartsWith(modulePrefix)).ToList();

            var loadedFiles = loadedAssemblies.Select(a => a.CodeBase.Replace("file:///", "").Replace('/', Path.DirectorySeparatorChar)).ToArray();

            var existingFiles = Directory.GetFiles(currentDomain.BaseDirectory, $"{modulePrefix}*.dll");

            var filesToLoad = existingFiles.Where(r => !loadedFiles.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();

            filesToLoad.ForEach(path => loadedAssemblies.Add(currentDomain.Load(AssemblyName.GetAssemblyName(path))));

            return loadedAssemblies.ToArray();
        }

        protected static void RegisterCommonModuleTypes(ContainerBuilder builder, string modulePrefix)
        {
            Assembly[] appAssemblies = LoadModuleAssemblies(modulePrefix);

            foreach (var asm in appAssemblies)
            {
                builder.RegisterAssemblyTypes(asm)
                    .Where(t => t.Name.EndsWith("DataHelper"))
                    .InstancePerLifetimeScope();

                builder.RegisterAssemblyTypes(asm)
                    .Where(t => t.Name.EndsWith("DataMapper"))
                    .InstancePerLifetimeScope();

                builder.RegisterAssemblyTypes(asm)
                    .Where(t => t.Name.EndsWith("Repository"))
                    .InstancePerLifetimeScope()
                    .AsSelf()
                    .AsImplementedInterfaces();

                builder.RegisterAssemblyTypes(asm)
                    .Where(t => t.Name.EndsWith("ManagerHelper"))
                    .InstancePerLifetimeScope();
            }
        }
    }
}
