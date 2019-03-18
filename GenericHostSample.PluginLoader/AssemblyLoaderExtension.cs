using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using GenericHostSample.PluginLoader.Internals;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace GenericHostSample.PluginLoader
{
    public static class AssemblyLoaderExtension
    {
        public static IHostBuilder ConfigureAssemblyScanning(this IHostBuilder hostBuilder, Action<AssemblyScannerOptions> configureAction = null)
        {
            AssemblyScannerOptions options = new AssemblyScannerOptions();
            configureAction?.Invoke(options);

            foreach (var optionsPluginPath in options.PluginPaths)
            {
                var plugin = LoadPlugin(options, optionsPluginPath);
                if (plugin == null)
                {
                    continue;
                }
                plugin.ConfigureHost(hostBuilder);
            }

            if (options.Scan)
            {
                foreach (var optionsPluginPath in Directory.EnumerateFiles(options.Root, options.PluginPattern, SearchOption.AllDirectories))
                {
                    var plugin = LoadPlugin(options, optionsPluginPath);
                    if (plugin == null)
                    {
                        continue;
                    }
                    plugin.ConfigureHost(hostBuilder);
                }

            }
            return hostBuilder;
        }


        private static IPlugin LoadPlugin(AssemblyScannerOptions options, string relativePath)
        {
            string pluginLocation = Path.GetFullPath(Path.Combine(options.Root, relativePath.Replace('\\', Path.DirectorySeparatorChar)));
            if (!File.Exists(pluginLocation))
            {
                if (!options.SuppressMessages)
                {
                    Console.WriteLine($"Can't find: {pluginLocation}");
                }
                return null;
            }
            if (!options.SuppressMessages)
            {
                Console.WriteLine($"Loading plugin from: {pluginLocation}");
            }

            PluginLoadContext loadContext = new PluginLoadContext(pluginLocation);
            var assembly = loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));

            var interfaceType = typeof(IPlugin);
            foreach(var type in assembly.GetExportedTypes())
            {
                if (!type.GetInterfaces().Contains(interfaceType))
                {
                    continue;
                }
                var plugin = Activator.CreateInstance(type) as IPlugin;
                return plugin;
            }
            return null;
        }
    }
}
