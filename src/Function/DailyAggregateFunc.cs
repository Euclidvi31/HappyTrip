using HappyTrip.Crawler;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;

namespace HappyTrip.Function
{
    public static class DailyAggregateFunc
    {
        [FunctionName("AggregateRun")]
        public static void Run([TimerTrigger("0 30 15 * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"AggregateRun start at: {DateTime.Now}.");

            try
            {
                var time = DateTime.UtcNow;
                CrawlerStorage.UpdateHistory(time).ConfigureAwait(false).GetAwaiter().GetResult();
                log.LogInformation("Traffic data update to database successfully.");

                var crawler = new HolidayCrawler();
                crawler.UpdateDatabase(time);
                log.LogInformation("Holiday data update to database successfully.");
            }
            catch (Exception e)
            {
                log.LogError($"Update to database error, e = {e.ToString()}");
                throw;
            }
            log.LogInformation($"AggregateRun finish.");
        }
    }
}
