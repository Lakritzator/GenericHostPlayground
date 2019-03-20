﻿using System.IO;
using System.Threading.Tasks;
using GenericHostSample.PluginLoader;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GenericHostSample.Console
{
    /// <summary>
    /// This demonstrates loading plugins
    /// </summary>
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureAssemblyScanning(assemblyScannerOptions =>
                {
                    assemblyScannerOptions.PluginPattern = "GenericHostSample.Plugin.OriginalSample.dll";
                    assemblyScannerOptions.Scan = true;
                })
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile("hostsettings.json", optional: true);
                    configHost.AddEnvironmentVariables(prefix: "PREFIX_");
                    configHost.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.AddJsonFile("appsettings.json", optional: true);
                    configApp.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                    configApp.AddEnvironmentVariables(prefix: "PREFIX_");
                    configApp.AddCommandLine(args);
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.AddConsole();
                    configLogging.AddDebug();
                })
                .UseConsoleLifetime()
                .Build();

            await host.RunAsync();
        }
    }
}