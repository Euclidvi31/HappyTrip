using System;
using System.ComponentModel.DataAnnotations;

namespace HappyTrip.Model
{
    public class PoiHistory
    {
        public int PoiId { get; set; }

        public int Date { get; set; }

        public int MinTraffic { get; set; }

        public int AvgTraffic { get; set; }

        public int MaxTraffic { get; set; }

        [MaxLength(10)]
        public string Status { get; set; }

        [MaxLength(20)]
        public string Whether { get; set; }
    }
}
