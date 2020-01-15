namespace HappyTripCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = TrafficCrawler.GetPOITrafficData().ConfigureAwait(false).GetAwaiter().GetResult();
            CrawlerStorage.SaveToBlob(data).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
