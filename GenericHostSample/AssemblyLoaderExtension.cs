using System;
using GenericHostSample.Scanner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace GenericHostSample
{
    public static class AssemblyLoaderExtension
    {
        public static IHostBuilder ConfigureAssemblyScanning(this IHostBuilder hostBuilder, Action<AssemblyScannerOptions> configureAction = null)
        {
            AssemblyScannerOptions options = new AssemblyScannerOptions();
            configureAction?.Invoke(options);
            hostBuilder.ConfigureServices((context, collection) => collection.TryAddSingleton(options));
            return hostBuilder.ConfigureServices((context, collection) => collection.AddSingleton<IHostedService, AssemblyScanner>());
        }
    }
}
