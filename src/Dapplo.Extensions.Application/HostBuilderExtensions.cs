using System;
using Dapplo.Extensions.Application.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dapplo.Extensions.Application
{
    /// <summary>
    /// Extensions for loading plugins
    /// </summary>
    public static class HostBuilderApplicationExtensions
    {
        /// <summary>
        /// Prevent that an application runs multiple times
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        /// <param name="mutexId">Mutex ID</param>
        /// <param name="whenNotFirstInstance">Action which is called when the mutex can't be locked</param>
        /// <param name="global">bool specifying if the mutex is global, one instance on a Windows instance, or local one instance per session (is default)</param>
        /// <returns>IHostBuilder for fluently calling</returns>
        public static IHostBuilder ForceSingleInstance(this IHostBuilder hostBuilder, string mutexId, Action<IHostingEnvironment> whenNotFirstInstance = null, bool global = false)
        {
            hostBuilder.ConfigureServices((hostContext, serviceCollection) =>
            {
                serviceCollection.AddSingleton(new MutexConfig
                {
                    MutexId = mutexId,
                    WhenNotFirstInstance = whenNotFirstInstance,
                    IsGlobal = global
                });
                serviceCollection.AddHostedService<MutexLifetimeService>();
            });
            return hostBuilder;
        }
    }
}
