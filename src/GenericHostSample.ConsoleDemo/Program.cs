using System;
using System.IO;
using System.Threading.Tasks;
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
        public static async Task Main(string[] args)
        {
            var host = new HostBuilder()
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
    }
}