using System;
using System.Globalization;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HappyTripCrawler
{
    public class TrafficCrawler
	{
		static readonly HttpClient client = new HttpClient();
        const int MaxRetry = 3;

		public static async Task<string> GetPOITrafficData()
		{
            string result = string.Empty;
            int currentRetry = 0;

            while (currentRetry < MaxRetry)
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync("https://shanghaicity.openservice.kankanews.com/public/tour/filterinfo2");
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();
                    result = ConvertEncoding(content);
                    break;
                }
                catch (Exception e)
                {
                    currentRetry++;
                    result = e.ToString();
                }
            }
            return result;
        }

        public static string ConvertEncoding(string body)
        {
            return Regex.Replace(
                body,
                @"\\[Uu]([0-9A-Fa-f]{4})",
                m => char.ToString(
                    (char)ushort.Parse(m.Groups[1].Value, NumberStyles.AllowHexSpecifier)));
        }
	}
}
