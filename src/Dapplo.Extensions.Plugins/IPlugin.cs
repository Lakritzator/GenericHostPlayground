using Microsoft.Extensions.Hosting;

namespace Dapplo.Extensions.Plugins
{
    public interface IPlugin
    {
        void ConfigureHost(IHostBuilder hostBuilder);
    }
}
