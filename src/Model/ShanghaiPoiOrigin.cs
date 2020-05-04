using System;
using System.Text.Json.Serialization;

namespace HappyTrip.Model
{
    public class ShanghaiPoiOrigin
    {
        // CODE
        [JsonPropertyName("CODE")]
        public string Code { get; set; }

        // NAME
        [JsonPropertyName("NAME")]
        public string Name { get; set; }

        // TIME
        [JsonPropertyName("TIME")]
        public string Time { get; set; }

        // R_TIME
        [JsonPropertyName("R_TIME")]
        public string RTime { get; set; }

        public DateTime RefreshAt => string.IsNullOrWhiteSpace(RTime) ? DateTime.Now : DateTime.Parse(RTime);

        // NUM
        [JsonPropertyName("NUM")]
        public string Num { get; set; }

        // MAX_NUM
        [JsonPropertyName("MAX_NUM")]
        public string MaxNum { get; set; }

        // NUM
        public int TrafficNumber => string.IsNullOrWhiteSpace(Num) ? 0 :  Convert.ToInt32(Num);

        // MAX_NUM
        public int MaxTrafficNumber => string.IsNullOrWhiteSpace(MaxNum) ? 0 : Convert.ToInt32(MaxNum);

        // TYPE
        [JsonPropertyName("TYPE")]
        public string Status { get; set; }

        // COUNTY
        [JsonPropertyName("COUNTY")]
        public string County { get; set; }

        // GRADE
        [JsonPropertyName("GRADE")]
        public string Grade { get; set; }

        // INITIAL
        [JsonPropertyName("INITIAL")]
        public string Initial { get; set; }

        // START_TIME
        [JsonPropertyName("START_TIME")]
        public string StartTime { get; set; }

        // END_TIME
        [JsonPropertyName("END_TIME")]
        public string EndTime { get; set; }

        // SSD
        [JsonPropertyName("SSD")]
        public string Comfort { get; set; }
    }
}
