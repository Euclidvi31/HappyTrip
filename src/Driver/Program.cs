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
            UpdateHolidayInformationFromStartToNow();
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

        private static void UpdatePoiHistoryFromStartToNow()
        {
            var startDate = new DateTime(2020, 6, 10);
            var endDate = DateTime.UtcNow;
            for (var date = startDate; date < endDate; date = date.AddDays(1))
            {
                CrawlerStorage.UpdateHistory(date).ConfigureAwait(false).GetAwaiter().GetResult();
                Console.WriteLine($"Finish {date}.");
            }
            Console.WriteLine("Done.");
        }

        private static void UpdateHolidayInformationFromStartToNow()
        {
            var startDate = new DateTime(2020, 1, 1);
            var endDate = DateTime.UtcNow;
            var crawler = new HolidayCrawler();
            for (var date = startDate; date < endDate; date = date.AddDays(1))
            {
                crawler.UpdateDatabase(date);
                Console.WriteLine($"Finish {date}.");
            }
            Console.WriteLine("Done.");
        }

        private static void CheckIsHoliday()
        {
            var crawler = new HolidayCrawler();
            var result = crawler.IsHoliday(DateTime.Parse("2020-07-25"));
            Console.WriteLine($"Is holiday: {result}");
        }
    }
}
