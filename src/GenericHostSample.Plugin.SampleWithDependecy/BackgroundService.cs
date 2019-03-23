using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Dapplo.HttpExtensions;

namespace GenericHostSample.Plugin.SampleWithDependecy
{
    internal class BackgroundService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer;

        public BackgroundService(ILogger<BackgroundService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation("Retrieving something.");

            Task.Run(async () =>
            {
                var result = await (new Uri("https://nu.nl").GetAsAsync<string>());
                _logger.LogInformation(result.Substring(0,40));
            });
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
