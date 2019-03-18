using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace GenericHostSample.PluginLoader.Scanner
{
    /// <summary>
    /// This enables assembly scanning
    /// </summary>
    public class AssemblyScanner : IHostedService
    {
        private IApplicationLifetime ApplicationLifetime { get; }
        private AssemblyScannerOptions Options { get; }
        private IHostingEnvironment Environment { get; }

        public AssemblyScanner(IOptions<AssemblyScannerOptions> options, IHostingEnvironment environment, IApplicationLifetime applicationLifetime)
        {
            Options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            Environment = environment ?? throw new ArgumentNullException(nameof(environment));
            ApplicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (!Options.SuppressMessages)
            {
                Console.WriteLine("Started!");
            }

            foreach (var optionsPluginPath in Options.PluginPaths)
            {
                LoadPlugin(optionsPluginPath);
            }

            if (Options.Scan)
            {
                foreach (var optionsPluginPath in Directory.EnumerateFiles(Options.Root, Options.PluginPattern, SearchOption.AllDirectories))
                {
                    LoadPlugin(optionsPluginPath);
                }

            }
            // Console applications start immediately.
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // There's nothing to do here
            return Task.CompletedTask;
        }

        private Assembly LoadPlugin(string relativePath)
        {
            string pluginLocation = Path.GetFullPath(Path.Combine(Options.Root, relativePath.Replace('\\', Path.DirectorySeparatorChar)));
            if (!Options.SuppressMessages)
            {
                Console.WriteLine($"Loading plugin from: {pluginLocation}");
            }
            PluginLoadContext loadContext = new PluginLoadContext(pluginLocation);
            return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));
        }

        private void Configure(Assembly assembly)
        {

        }

    }
}
