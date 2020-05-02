using HappyTrip.Crawler;
using HappyTrip.Model;
using System;

namespace HappyTrip.Driver
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = TrafficCrawler.GetPOITrafficData().ConfigureAwait(false).GetAwaiter().GetResult();
            // CrawlerStorage.SaveToBlob(data).ConfigureAwait(false).GetAwaiter().GetResult();
            CrawlerStorage.UpdateToDatabase(data).GetAwaiter().GetResult();
            Console.WriteLine("Done.");
        }
    }
}
