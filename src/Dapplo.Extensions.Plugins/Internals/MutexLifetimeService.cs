using System.Threading;
using System.Threading.Tasks;
using Dapplo.Extensions.Plugins;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GenericHostSample.Plugin.OriginalSample
{
    internal class MutexLifetimeService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IApplicationLifetime _appLifetime;
        private readonly ResourceMutex _resourceMutex;

        public MutexLifetimeService(ILogger logger, IApplicationLifetime appLifetime, ResourceMutex resourceMutex)
        {
            _logger = logger;
            _appLifetime = appLifetime;
            _resourceMutex = resourceMutex;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStopping.Register(OnStopping);

            return Task.CompletedTask;
        }


        private void OnStopping()
        {
            _logger.LogInformation("OnStopping has been called.");
            _resourceMutex.Dispose();

            // Perform on-stopping activities here
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}