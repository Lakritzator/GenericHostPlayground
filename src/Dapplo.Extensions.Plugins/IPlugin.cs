using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dapplo.Extensions.Plugins
{
    /// <summary>
    /// This interface is the connection between the host and the plugin code
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Implementing this method allows a plugin to configure the host.
        /// This makes it possible to add services etc
        /// </summary>
        /// <param name="hostBuilderContext">HostBuilderContext</param>
        /// <param name="serviceCollection">IServiceCollection</param>
        void ConfigureHost(HostBuilderContext hostBuilderContext, IServiceCollection serviceCollection);
    }
}
