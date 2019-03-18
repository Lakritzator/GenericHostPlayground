using Microsoft.Extensions.DependencyInjection;

namespace GenericHostSample.Plugin1
{
    /// <summary>
    /// Init class for the plugin
    /// </summary>
    public class Plugin
    {
        /// <summary>
        /// This defines all services
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<LifetimeEventsHostedService>();
            services.AddHostedService<TimedHostedService>();
        }
    }
}
