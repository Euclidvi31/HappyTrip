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
            var data = TrafficCrawler.GetPOITrafficData().ConfigureAwait(false).GetAwaiter().GetResult();
            // CrawlerStorage.SaveToBlob(data).ConfigureAwait(false).GetAwaiter().GetResult();
            CrawlerStorage.UpdateToDatabase(data).GetAwaiter().GetResult();
            Console.WriteLine("Done.");

            //int[] numbers = { 0, 30, 20, 15, 90, 85, 40, 75 };

            //IEnumerable<int> query =
            //    numbers.Where((number, index) => number <= -1);

            //foreach (int number in query)
            //{
            //    Console.WriteLine(number);
            //}
        }
    }
}
