using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenericHostSample.FormsDemo
{
    /// <summary>
    /// Listens for Ctrl+C or SIGTERM and initiates shutdown.
    /// </summary>
    public class FormsLifeTime : IHostLifetime, IDisposable
    {
        private readonly ManualResetEvent _shutdownBlock = new ManualResetEvent(false);

        public FormsLifeTime(IOptions<ConsoleLifetimeOptions> options, IHostingEnvironment environment, IApplicationLifetime applicationLifetime)
        {
            Options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            Environment = environment ?? throw new ArgumentNullException(nameof(environment));
            ApplicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
        }

        private ConsoleLifetimeOptions Options { get; }

        private IHostingEnvironment Environment { get; }

        private IApplicationLifetime ApplicationLifetime { get; }

        public Task WaitForStartAsync(CancellationToken cancellationToken)
        {
            if (!Options.SuppressStatusMessages)
            {
                ApplicationLifetime.ApplicationStarted.Register(() =>
                {
                    Debug.WriteLine("Application started.");
                    Debug.WriteLine($"Hosting environment: {Environment.EnvironmentName}");
                    Debug.WriteLine($"Content root path: {Environment.ContentRootPath}");
                });
            }

            Application.ApplicationExit += (sender, eventArgs) =>
            {
                ApplicationLifetime.StopApplication();
                _shutdownBlock.WaitOne();
            };
            // Console applications start immediately.
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // There's nothing to do here
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _shutdownBlock.Set();
        }
    }
}
