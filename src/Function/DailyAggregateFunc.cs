using HappyTrip.Crawler;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;

namespace HappyTrip.Function
{
    public static class DailyAggregateFunc
    {
        [FunctionName("AggregateRun")]
        public static void Run([TimerTrigger("0 30 15 * * *", RunOnStartup = true)]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"AggregateRun start at: {DateTime.Now}.");

            try
            {
                CrawlerStorage.UpdateHistory(DateTime.UtcNow).ConfigureAwait(false).GetAwaiter().GetResult();
                log.LogInformation("Data update to database successfully.");
            }
            catch (Exception e)
            {
                log.LogError($"Update to database error, e = {e.ToString()}");
            }
            log.LogInformation($"AggregateRun finish.");
        }
    }
}
