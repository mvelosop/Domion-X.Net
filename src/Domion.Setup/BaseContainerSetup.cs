using Autofac;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

#if NETCOREAPP1_1
using Microsoft.Extensions.DependencyModel;
#endif

namespace Domion.Setup
{
    public class BaseContainerSetup
    {
        protected static Assembly[] LoadModuleAssemblies(string modulePrefix)
        {
            var loadedAssemblies = new List<Assembly>();

#if NETCOREAPP1_1

            // Adapted from: http://www.michael-whelan.net/replacing-appdomain-in-dotnet-core/

            bool IsCandidateLibrary(RuntimeLibrary library) =>
                library.Name == (modulePrefix) || library.Dependencies.Any(d => d.Name.StartsWith(modulePrefix));

            string BaseDirectory() => AppContext.BaseDirectory;

            IReadOnlyList<RuntimeLibrary> dependencies = DependencyContext.Default.RuntimeLibraries;

            foreach (RuntimeLibrary library in dependencies)
            {
                if (!IsCandidateLibrary(library)) continue;

                loadedAssemblies.Add(Assembly.Load(new AssemblyName(library.Name)));
            }

            string[] loadedFiles = loadedAssemblies.Select(a => a.CodeBase.Replace("file:///", "").Replace('/', Path.DirectorySeparatorChar)).ToArray();

            var existingFiles = Directory.GetFiles(BaseDirectory(), $"{modulePrefix}*.dll");

            var filesToLoad = existingFiles.Where(r => !loadedFiles.Contains(r, StringComparer.CurrentCultureIgnoreCase)).ToList();

            filesToLoad.ForEach(name => loadedAssemblies.Add(Assembly.Load(new AssemblyName(name))));

#else

            // From: https://stackoverflow.com/questions/2384592/is-there-a-way-to-force-all-referenced-assemblies-to-be-loaded-into-the-app-doma
            // With some fixes!

            AppDomain currentDomain = AppDomain.CurrentDomain;

            loadedAssemblies = currentDomain.GetAssemblies().Where(a => a.FullName.StartsWith(modulePrefix)).ToList();

            string[] loadedFiles = loadedAssemblies.Select(a => a.CodeBase.Replace("file:///", "").Replace('/', Path.DirectorySeparatorChar)).ToArray();

            var existingFiles = Directory.GetFiles(currentDomain.BaseDirectory, $"{modulePrefix}*.dll");

            var filesToLoad = existingFiles.Where(r => !loadedFiles.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();

            filesToLoad.ForEach(path => loadedAssemblies.Add(currentDomain.Load(AssemblyName.GetAssemblyName(path))));

#endif

            return loadedAssemblies.ToArray();
        }

        protected static void RegisterCommonModuleTypes(ContainerBuilder builder, string modulePrefix)
        {
            Assembly[] appAssemblies = LoadModuleAssemblies(modulePrefix);

            foreach (Assembly assembly in appAssemblies)
            {
                builder.RegisterAssemblyTypes(assembly)
                    .Where(t => t.Name.EndsWith("DataHelper"))
                    .InstancePerLifetimeScope();

                builder.RegisterAssemblyTypes(assembly)
                    .Where(t => t.Name.EndsWith("DataMapper"))
                    .InstancePerLifetimeScope();

                builder.RegisterAssemblyTypes(assembly)
                    .Where(t => t.Name.EndsWith("Manager"))
                    .InstancePerLifetimeScope()
                    .AsSelf()
                    .AsImplementedInterfaces();

                builder.RegisterAssemblyTypes(assembly)
                    .Where(t => t.Name.EndsWith("ManagerHelper"))
                    .InstancePerLifetimeScope();

                builder.RegisterAssemblyTypes(assembly)
                    .Where(t => t.Name.EndsWith("Repository"))
                    .InstancePerLifetimeScope()
                    .AsSelf()
                    .AsImplementedInterfaces();

                builder.RegisterAssemblyTypes(assembly)
                    .Where(t => t.Name.EndsWith("RepositoryHelper"))
                    .InstancePerLifetimeScope();
            }
        }
    }
}
