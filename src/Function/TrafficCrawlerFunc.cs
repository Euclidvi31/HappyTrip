using HappyTrip.Crawler;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;

namespace HappyTrip.Function
{
    public static class TrafficCrawlerFunc
    {

        [FunctionName("CrawlerRun")]
        public static void Run([TimerTrigger("0 0 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"CrawlerRun start at: {DateTime.Now}.");

            var data = TrafficCrawler.GetPOITrafficData().ConfigureAwait(false).GetAwaiter().GetResult();

            try
            {
                CrawlerStorage.SaveToBlob(data).ConfigureAwait(false).GetAwaiter().GetResult();
                log.LogInformation($"Data save into blob successfully.");
            }
            catch (Exception e)
            {
                log.LogError($"Save to blob error, e = {e.ToString()}");
                throw;
            }

            try
            {
                CrawlerStorage.UpdateToDatabase(data).ConfigureAwait(false).GetAwaiter().GetResult();
                log.LogInformation("Data update to database successfully.");
            }
            catch (Exception e)
            {
                log.LogError($"Update to database error, e = {e.ToString()}");
                throw;
            }
            log.LogInformation($"CrawlerRun finish.");
        }
    }
}
