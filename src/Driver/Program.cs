using HappyTrip.Crawler;
using HappyTrip.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HappyTrip.Driver
{
    class Program
    {
        static void Main(string[] args)
        {
            
        }

        private static void GetAndUpdatePoiData()
        {
            var data = TrafficCrawler.GetPOITrafficData().ConfigureAwait(false).GetAwaiter().GetResult();
            CrawlerStorage.SaveToBlob(data).ConfigureAwait(false).GetAwaiter().GetResult();
            CrawlerStorage.UpdateToDatabase(data).GetAwaiter().GetResult();
            Console.WriteLine("Done.");
        }

        private static void GetAndUpdatePoiHistory()
        {
            CrawlerStorage.UpdateHistory(DateTime.UtcNow).ConfigureAwait(false).GetAwaiter().GetResult();
            Console.WriteLine("Done.");
        }
    }
}
