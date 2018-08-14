using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;

namespace TvMazeScraper.ScraperStarterFunction
{
    public static class ScraperRunner
    {
        // Runs every day at 01:00 AM
        [FunctionName("ScraperRunner")]
        public static void Run([TimerTrigger("0 0 1 * * *")]TimerInfo myTimer, 
                                TraceWriter log,
                                [ServiceBus("servicebusqueue", Connection = "ServiceBusConnectionString", EntityType = EntityType.Queue)]out string queueMessage)
        {            
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
            queueMessage = "StartScraper";
        }

    }
}
