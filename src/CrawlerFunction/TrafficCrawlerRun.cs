using HappyTripCrawler;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;

namespace CrawlerFunction
{
    public static class TrafficCrawlerRun
    {

        [FunctionName("CrawlerRun")]
        public static void Run([TimerTrigger("0 0 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"CrawlerRun start at: {DateTime.Now}.");

            var data = TrafficCrawler.GetPOITrafficData().ConfigureAwait(false).GetAwaiter().GetResult();
            CrawlerStorage.SaveToBlob(data).ConfigureAwait(false).GetAwaiter().GetResult();
            log.LogInformation($"CrawlerRun finish.");
        }
    }
}
