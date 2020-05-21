using AutoMapper;
using Azure.Storage.Blobs;
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
        }

        public static async Task<Dictionary<int, PoiHistory>> UpdateHistory(DateTime day)
        {
            const string connectionString = "DefaultEndpointsProtocol=https;AccountName=trafficblob;AccountKey=f798gA7BzmMev2qB0HIEuC7rx2uHoHqgWY04lrkvL+WniDX2eg7Zdi4rlOgmMlqh3ZGED9EUMyy4bs5EjnN3+w==;EndpointSuffix=core.windows.net";
            const string containerName = "traficdata";

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();
            // TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById("Asia/Shanghai");
            TimeZoneInfo timeInfo = TimeZoneInfo.CreateCustomTimeZone("ShanghaiTime", new TimeSpan(08, 00, 00), "ShanghaiTime", "ShanghaiTime");
            string fileName = TimeZoneInfo.ConvertTime(day, timeInfo).ToString("yyyyMMdd");

        }
    }
}
