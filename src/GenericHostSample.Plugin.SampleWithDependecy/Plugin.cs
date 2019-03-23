using Dapplo.Extensions.Plugins;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace GenericHostSample.Plugin.SampleWithDependecy
{
    public class Plugin : IPlugin
    {
        public void ConfigureHost(IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(serviceCollection =>
            {
                serviceCollection.AddHostedService<BackgroundService>();
            });
        }
    }
}
