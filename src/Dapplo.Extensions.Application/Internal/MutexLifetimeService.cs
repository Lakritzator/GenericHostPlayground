using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dapplo.Extensions.Application.Internal
{
    /// <summary>
    /// This maintains the mutex lifetime
    /// </summary>
    internal class MutexLifetimeService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IApplicationLifetime _appLifetime;
        private readonly MutexConfig _mutexConfig;
        private ResourceMutex _resourceMutex;

        public MutexLifetimeService(ILogger<MutexLifetimeService> logger, IHostingEnvironment hostingEnvironment, IApplicationLifetime appLifetime, MutexConfig mutexConfig)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _appLifetime = appLifetime;
            _mutexConfig = mutexConfig;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _resourceMutex = ResourceMutex.Create(null, _mutexConfig.MutexId, _hostingEnvironment.ApplicationName, _mutexConfig.IsGlobal);

            _appLifetime.ApplicationStopping.Register(OnStopping);
            if (!_resourceMutex.IsLocked)
            {
                _mutexConfig.WhenNotFirstInstance?.Invoke(_hostingEnvironment);
                _logger.LogDebug("Application {0} already running, stopping application.", _hostingEnvironment.ApplicationName);
                _appLifetime.StopApplication();
            }

            return Task.CompletedTask;
        }


        private void OnStopping()
        {
            _logger.LogInformation("OnStopping has been called, closing mutex.");
            _resourceMutex.Dispose();
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}