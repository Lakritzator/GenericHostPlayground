using GenericHostSample.PluginLoader;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GenericHostSample.Plugin1
{
    /// <summary>
    /// Init class for the plugin
    /// </summary>
    public class Plugin : IPlugin
    {
        public void ConfigureHost(IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(serviceCollection =>
            {
                serviceCollection.AddHostedService<LifetimeEventsHostedService>();
                serviceCollection.AddHostedService<TimedHostedService>();
            });
        }
    }
}
