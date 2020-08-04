using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using HappyTrip.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HappyTrip.Crawler
{
    public class CrawlerStorage
    {
        public static async Task SaveToBlob(string data)
        {
            const string connectionString = "DefaultEndpointsProtocol=https;AccountName=trafficblob;AccountKey=f798gA7BzmMev2qB0HIEuC7rx2uHoHqgWY04lrkvL+WniDX2eg7Zdi4rlOgmMlqh3ZGED9EUMyy4bs5EjnN3+w==;EndpointSuffix=core.windows.net";
            const string containerName = "traficdata";

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();
            // TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById("Asia/Shanghai");
            TimeZoneInfo timeInfo = TimeZoneInfo.CreateCustomTimeZone("ShanghaiTime", new TimeSpan(08, 00, 00), "ShanghaiTime", "ShanghaiTime");
            string fileName = TimeZoneInfo.ConvertTime(DateTime.Now, timeInfo).ToString("yyyyMMdd-HHmmss");
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(data)))
            {
                await blobClient.UploadAsync(ms);
            }
        }

        public static async Task UpdateToDatabase(string data)
        {
            var poiOrigins = JsonSerializer.Deserialize<ShanghaiPoiOrigin[]>(data);
            var configuration = new MapperConfiguration(cfg => cfg.CreateMap<ShanghaiPoiOrigin, Poi>());
            var mapper = configuration.CreateMapper();
            Poi[] pois = mapper.Map<ShanghaiPoiOrigin[], Poi[]>(poiOrigins);
            using (var context = new PoiContext())
            {
                foreach (var poi in pois)
                {
                    var existingPoi = context.Poi.SingleOrDefault(p => p.Code == poi.Code);
                    if (existingPoi != null)
                    {
                        existingPoi.RefreshAt = poi.RefreshAt;
                        existingPoi.TrafficNumber = poi.TrafficNumber;
                        existingPoi.MaxTrafficNumber = poi.MaxTrafficNumber;
                        existingPoi.Status = poi.Status;
                        existingPoi.County = poi.County;
                        existingPoi.Grade = poi.Grade;
                        existingPoi.Initial = poi.Initial;
                        existingPoi.StartTime = poi.StartTime;
                        existingPoi.EndTime = poi.EndTime;
                        existingPoi.Comfort = poi.Comfort;
                    }
                    else
                    {
                        context.Poi.Add(poi);
                    }
                }
                await context.SaveChangesAsync();
            }

            using (var context = new PoiContext())
            {
                var tripPoi = await context.Poi.FindAsync(26);
                var disneyPoi = await context.Poi.FindAsync(48);
                var yioulaiPoi = await context.Poi.FindAsync(44);
                disneyPoi.TrafficNumber = (int)(tripPoi.TrafficNumber * 0.97);
                disneyPoi.MaxTrafficNumber = 70000;
                yioulaiPoi.TrafficNumber = (int)(tripPoi.TrafficNumber * 0.03);
                yioulaiPoi.MaxTrafficNumber = 16000;

                await context.SaveChangesAsync();
            }
        }

        public static async Task UpdateHistory(DateTime day)
        {
            const string connectionString = "DefaultEndpointsProtocol=https;AccountName=trafficblob;AccountKey=f798gA7BzmMev2qB0HIEuC7rx2uHoHqgWY04lrkvL+WniDX2eg7Zdi4rlOgmMlqh3ZGED9EUMyy4bs5EjnN3+w==;EndpointSuffix=core.windows.net";
            const string containerName = "traficdata";

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();
            // TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById("Asia/Shanghai");
            TimeZoneInfo timeInfo = TimeZoneInfo.CreateCustomTimeZone("ShanghaiTime", new TimeSpan(08, 00, 00), "ShanghaiTime", "ShanghaiTime");
            string fileName = TimeZoneInfo.ConvertTime(day, timeInfo).ToString("yyyyMMdd");
            int dateInt = Convert.ToInt32(fileName);
            var blobs = containerClient.GetBlobsAsync(BlobTraits.None, BlobStates.None, fileName);
            var enumerator = blobs.GetAsyncEnumerator();
            var poiCollection = new Dictionary<string, List<Poi>>();
            while (await enumerator.MoveNextAsync())
            {
                var current = enumerator.Current;
                var blobName = current.Name;
                var blobClient = containerClient.GetBlobClient(blobName);
                var pois = await GetPoisFromBlob(blobClient);
                ProcessPois(pois, poiCollection);
            }

            if (poiCollection.Count == 0)
            {
                throw new Exception("Cannot get poi collection of the day from blob.");
            }

            var result = new List<PoiHistory>();
            foreach (var item in poiCollection)
            {
                var poiCode = item.Key;
                var poiList = item.Value;
                var history = CalculatePoiHistory(poiCode, poiList, dateInt);
                result.Add(history);
            }
            await UpdatePoiHistory(result);
            await UpdateHardCodedPoiHistory(dateInt);
        }

        private static async Task<Poi[]> GetPoisFromBlob(BlobClient blobClient)
        {
            var configuration = new MapperConfiguration(cfg => cfg.CreateMap<ShanghaiPoiOrigin, Poi>());
            var mapper = configuration.CreateMapper();
            var blobContent = await blobClient.DownloadAsync();
            try
            {
                using (var s = blobContent.Value.Content)
                {
                    var poiOrigins = await JsonSerializer.DeserializeAsync<ShanghaiPoiOrigin[]>(s);
                    Poi[] pois = mapper.Map<ShanghaiPoiOrigin[], Poi[]>(poiOrigins);
                    return pois;
                }
            }
            catch (Exception)
            {
                return new Poi[0];
            }
        }

        private static void ProcessPois(Poi[] pois, Dictionary<string, List<Poi>> poiCollection)
        {
            foreach (var poi in pois)
            {
                if (!poiCollection.ContainsKey(poi.Code))
                {
                    poiCollection[poi.Code] = new List<Poi>();
                }
                poiCollection[poi.Code].Add(poi);
            }
        }

        private static PoiHistory CalculatePoiHistory(string code, List<Poi> pois, int date)
        {
            int poiId;
            using (var context = new PoiContext())
            {
                var existingPoi = context.Poi.SingleOrDefault(p => p.Code == code);
                poiId = existingPoi.Id;
            }
            var history = new PoiHistory()
            {
                PoiId = poiId,
                Date = date,
                MaxTraffic = pois.Max(p => p.TrafficNumber),
                MinTraffic = pois.Where(p => p.TrafficNumber >= 0).Min(p => p.TrafficNumber),
                TrafficLimit = pois.Max(p => p.MaxTrafficNumber),
            };

            if (history.MaxTraffic != 0)
            {
                history.AvgTraffic = (int)pois.Where(p => p.TrafficNumber > 0).Average(p => p.TrafficNumber);
            }
            return history;
        }

        private static async Task UpdatePoiHistory(List<PoiHistory> historys)
        {
            using (var context = new PoiContext())
            {
                foreach (var history in historys)
                {
                    var existing = context.PoiHistory.SingleOrDefault(p => p.PoiId == history.PoiId && p.Date == history.Date);
                    if (existing != null)
                    {
                        existing.MaxTraffic = history.MaxTraffic;
                        existing.MinTraffic = history.MinTraffic;
                        existing.AvgTraffic = history.AvgTraffic;
                        existing.Status = history.Status;
                        existing.Whether = history.Whether;
                    }
                    else
                    {
                        context.PoiHistory.Add(history);
                    }
                }

                await context.SaveChangesAsync();
            }
        }

        private static async Task UpdateHardCodedPoiHistory(int date)
        {
            using (var context = new PoiContext())
            {
                var tripPoi = await context.PoiHistory.FindAsync(26, date);
                var disneyPoi = await context.PoiHistory.FindAsync(48, date);
                var yioulaiPoi = await context.PoiHistory.FindAsync(44, date);
                disneyPoi.MaxTraffic = (int)(tripPoi.MaxTraffic * 0.97);
                disneyPoi.AvgTraffic = (int)(tripPoi.AvgTraffic * 0.97);
                disneyPoi.MinTraffic = (int)(tripPoi.MinTraffic * 0.97);

                yioulaiPoi.MaxTraffic = (int)(tripPoi.MaxTraffic * 0.03);
                yioulaiPoi.AvgTraffic = (int)(tripPoi.AvgTraffic * 0.03);
                yioulaiPoi.MinTraffic = (int)(tripPoi.MinTraffic * 0.03);
                await context.SaveChangesAsync();
            }
        }
    }
}
