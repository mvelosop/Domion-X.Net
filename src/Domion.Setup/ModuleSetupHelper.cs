using Autofac;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Domion.Setup
{
    public class ModuleSetupHelper
    {
        public static Assembly[] GetModuleAssemblies(string modulePrefix)
        {
            // From: https://stackoverflow.com/questions/2384592/is-there-a-way-to-force-all-referenced-assemblies-to-be-loaded-into-the-app-doma
            // With some fixes!

            AppDomain currentDomain = AppDomain.CurrentDomain;

            var loadedAssemblies = currentDomain.GetAssemblies().Where(a => a.FullName.StartsWith(modulePrefix)).ToList();

            var loadedPaths = loadedAssemblies.Select(a => a.CodeBase.Replace("file:///", "").Replace('/', Path.DirectorySeparatorChar)).ToArray();

            var referencedPaths = Directory.GetFiles(currentDomain.BaseDirectory, $"{modulePrefix}*.dll");

            var toLoad = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();

            toLoad.ForEach(path => loadedAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path))));

            return loadedAssemblies.ToArray();
        }

        public static void RegisterCommonModuleTypes(ContainerBuilder builder, string modulePrefix)
        {
            Assembly[] appAssemblies = GetModuleAssemblies(modulePrefix);

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
    }
}
