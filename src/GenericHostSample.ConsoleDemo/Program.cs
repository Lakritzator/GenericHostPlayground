using System;
using System.IO;
using System.Threading.Tasks;
using Dapplo.Extensions.Application;
using Dapplo.Extensions.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GenericHostSample.ConsoleDemo
{
    /// <summary>
    /// This demonstrates loading plugins
    /// </summary>
    public static class Program
    {
        private const string AppSettingsFilePrefix = "appsettings";
        private const string HostSettingsFile = "hostsettings.json";
        private const string Prefix = "PREFIX_";
        public static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureLogging()
                .ConfigureConfiguration(args)
                .ForceSingleInstance("{B9CE32C0-59AE-4AF0-BE39-5329AAFF4BE8}", (hostingEnvironment) => {
                    // This is called when a second instance is started
                    Console.WriteLine($"Application {hostingEnvironment.ApplicationName} already running.");
                    Console.ReadKey();
                })
                // Specify the location from where the dll's are "globbed"
                .UseContentRoot(@"..\..\..\..\")
                // Add the plugins which can be found with the specified globs
                .AddPlugins(@"**\bin\**\*.Plugin.*.dll")
                .UseConsoleLifetime()
                .Build();

            Console.WriteLine("Run!");
            await host.RunAsync();
        }

        /// <summary>
        /// Configure the loggers
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        /// <returns>IHostBuilder</returns>
        private static IHostBuilder ConfigureLogging(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureLogging((hostContext, configLogging) =>
            {
                configLogging.AddConsole();
                configLogging.AddDebug();
            });
        }
        
        /// <summary>
        /// Configure the configuration
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static IHostBuilder ConfigureConfiguration(this IHostBuilder hostBuilder, string[] args)
        {
            return hostBuilder.ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile(HostSettingsFile, optional: true);
                    configHost.AddEnvironmentVariables(prefix: Prefix);
                    configHost.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.AddJsonFile(AppSettingsFilePrefix + ".json", optional: true);
                    if (!string.IsNullOrEmpty(hostContext.HostingEnvironment.EnvironmentName))
                    {
                        configApp.AddJsonFile(AppSettingsFilePrefix + $".{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                    }
                    configApp.AddEnvironmentVariables(prefix: Prefix);
                    configApp.AddCommandLine(args);
                });
        }
    }
}