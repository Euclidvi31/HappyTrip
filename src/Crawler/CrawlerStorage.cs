using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HappyTripCrawler
{
    public class CrawlerStorage
    {
        const string connectionString = "DefaultEndpointsProtocol=https;AccountName=trafficblob;AccountKey=f798gA7BzmMev2qB0HIEuC7rx2uHoHqgWY04lrkvL+WniDX2eg7Zdi4rlOgmMlqh3ZGED9EUMyy4bs5EjnN3+w==;EndpointSuffix=core.windows.net";
        const string containerName = "traficdata";

        public static async Task SaveToBlob(string data)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();
            TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById("Asia/Shanghai");
            string fileName = TimeZoneInfo.ConvertTime(DateTime.Now, timeInfo).ToString("yyyyMMdd-HHmmss");
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(data)))
            {
                await blobClient.UploadAsync(ms);
            }
        }
    }
}
