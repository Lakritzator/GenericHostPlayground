﻿using Dapplo.Extensions.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GenericHostSample.Plugin.OriginalSample
{
    /// <summary>
    /// Init class for the plugin
    /// </summary>
    public class Plugin : IPlugin
    {
        public void ConfigureHost(HostBuilderContext hostBuilderContext, IServiceCollection serviceCollection)
        {
            serviceCollection.AddHostedService<LifetimeEventsHostedService>();
            serviceCollection.AddHostedService<TimedHostedService>();
        }
    }
}
