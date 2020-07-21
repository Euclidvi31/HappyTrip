using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HappyTrip.Crawler.Model
{
    public class BaiduHolidayModel
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("data")]
        public List<ModelData> Data { get; set; }
    }

    public class ModelData
    {
        [JsonPropertyName("_update_time")]
        public string UpdateAt { get; set; }

        [JsonPropertyName("holiday")]
        public List<HolidayDetail> HolidayList { get; set; }
    }

    public class HolidayDetail
    {
        [JsonPropertyName("festival")]
        public string Festival { get; set; }

        [JsonPropertyName("list")]
        public List<DateDetail> DateList { get; set; }
    }

    public class DateDetail
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("date")]
        public string Date { get; set; }
    }
}
