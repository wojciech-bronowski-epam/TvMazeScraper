using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TvMazeScraper.ScraperServices
{
    public class ScraperRunnerHostedService : IHostedService
    {
        private readonly IQueueClient _queueClient;
        private readonly IServiceBusListener _serviceBusListener;
        private readonly ILogger<ScraperRunnerHostedService> _logger;

        public ScraperRunnerHostedService(IQueueClient queueClient, IServiceBusListener serviceBusListener, ILoggerFactory loggerFactory)
        {
            _queueClient = queueClient;
            _serviceBusListener = serviceBusListener;
            _logger = loggerFactory.CreateLogger<ScraperRunnerHostedService>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _serviceBusListener.Start();
            return Task.CompletedTask;
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Stopping ScraperRunnerHostedService...");
            await _queueClient.CloseAsync();
        }
    }
}
