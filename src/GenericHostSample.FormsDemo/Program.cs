using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dapplo.Extensions.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GenericHostSample.FormsDemo
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static async Task Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            var host = new HostBuilder()
                .AddPlugins(matcher =>
                {
                    matcher.AddInclude(@"..\..\..\..\**\bin\**\*.Plugin.*.dll");
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
                    configLogging.AddDebug();
                })
                .ConfigureServices(serviceCollection =>
                {
                    serviceCollection.AddSingleton<Form1>();
                })
                .UseFormsLifetime()
                .Build();

            await host.RunAsync();
        }

        /// <summary>
        /// Listens for Ctrl+C or SIGTERM and calls <see cref="IApplicationLifetime.StopApplication"/> to start the shutdown process.
        /// This will unblock extensions like RunAsync and WaitForShutdownAsync.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IHostBuilder" /> to configure.</param>
        /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
        public static IHostBuilder UseFormsLifetime(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices((context, collection) => collection.AddSingleton<IHostLifetime, FormsLifeTime>());
        }
    }
}
