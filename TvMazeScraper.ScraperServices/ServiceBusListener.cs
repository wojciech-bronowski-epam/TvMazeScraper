using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TvMazeScraper.ScraperServices.Scraper;

namespace TvMazeScraper.ScraperServices
{
    public class ServiceBusListener : IServiceBusListener
    {
        private readonly IQueueClient _queueClient;
        private readonly IServiceProvider _services;
        private readonly ILogger<ServiceBusListener> _logger;

        public ServiceBusListener(IQueueClient queueClient, IServiceProvider services, ILoggerFactory loggerFactory)
        {
            _queueClient = queueClient;
            _services = services;
            _logger = loggerFactory.CreateLogger<ServiceBusListener>();
        }

        public void Start()
        {
            _logger.LogDebug("Start ServiceBusListener");
            _queueClient.RegisterMessageHandler(ProcessMessagesAsync, new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false,
            });
        }

        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            _logger.LogDebug($"Received message with SequenceNumber:{message.SystemProperties.SequenceNumber}");
            _logger.LogDebug($"Starting scraper...");
            await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
            using (var scope = _services.CreateScope())
            {
                var scopedScraperService = scope.ServiceProvider.GetRequiredService<IScraperService>();
                await scopedScraperService.RunAsync(token);
            }
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs args)
        {
            _logger.LogDebug($"Exception: {args.Exception.Message}");
            return Task.CompletedTask;
        }

    }
}