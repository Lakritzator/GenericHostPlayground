﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;
using GenericHostSample.PluginLoader.Internals;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using GenericHostSample.Plugin.OriginalSample;

namespace Dapplo.Extensions.Plugins
{
    /// <summary>
    /// Extensions for loading plugins
    /// </summary>
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// This enables scanning with Plugins
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        /// <param name="configureAction">Action to configure where the plugins come from</param>
        /// <param name="scanRoot">string with root directory to scan the plugins, default the location of the exeutable</param>
        /// <returns>IHostBuilder for fluently calling</returns>
        public static IHostBuilder PreventMultipleInstances(this IHostBuilder hostBuilder, string mutexId, bool global = false)
        {
            hostBuilder.ConfigureServices(serviceCollection =>
            {
                serviceCollection.AddSingleton(ResourceMutex.Create(null, mutexId, null, global));
                serviceCollection.AddHostedService<MutexLifetimeService>();
            });
            return hostBuilder;
        }

        /// <summary>
        /// This enables scanning with Plugins
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        /// <param name="configureAction">Action to configure where the plugins come from</param>
        /// <returns>IHostBuilder for fluently calling</returns>
        public static IHostBuilder AddPlugins(this IHostBuilder hostBuilder, Action<Matcher> configureAction)
        {
            hostBuilder.ConfigureServices((hostContext, serviceCollection) =>
            {
                
                var matcher = new Matcher();
                configureAction?.Invoke(matcher);
                var scanRoot = hostContext.HostingEnvironment.ContentRootPath ?? Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                 
                foreach (var pluginPath in matcher.GetResultsInFullPath(scanRoot))
                {
                    var plugin = LoadPlugin(pluginPath);
                    if (plugin == null)
                    {
                        continue;
                    }
                    plugin.ConfigureHost(hostContext, serviceCollection);
                }
            });

            return hostBuilder;
        }

        /// <summary>
        /// Helper method to load an assembly which contains a single plugin
        /// </summary>
        /// <param name="pluginLocation">string</param>
        /// <returns>IPlugin</returns>
        private static IPlugin LoadPlugin(string pluginLocation)
        {
            if (!File.Exists(pluginLocation))
            {
                Console.WriteLine($"Can't find: {pluginLocation}");
                return null;
            }
            Console.WriteLine($"Loading plugin from: {pluginLocation}");
 
            PluginLoadContext loadContext = new PluginLoadContext(pluginLocation);
            var assembly = loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));

            var interfaceType = typeof(IPlugin);
            foreach (var type in assembly.GetExportedTypes())
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
