using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GenericHostSample.PluginLoader
{
    public interface IPlugin
    {
        void ConfigureHost(IHostBuilder hostBuilder);
    }
}
