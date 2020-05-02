using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HappyTrip.Model
{
    public class Poi
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // CODE
        [MaxLength(10)]
        public string Code { get; set; }

        // NAME
        [MaxLength(50)]
        public string Name { get; set; }

        // TIME
        public DateTime RefreshAt { get; set; }

        // NUM
        public int TrafficNumber { get; set; }

        // MAX_NUM
        public int MaxTrafficNumber { get; set; }

        // TYPE
        [MaxLength(10)]
        public string Status { get; set; }

        // COUNTY
        [MaxLength(10)]
        public string County { get; set; }

        // GRADE
        [MaxLength(8)]
        public string Grade { get; set; }

        // INITIAL
        [MaxLength(20)]
        public string Initial { get; set; }

        // START_TIME
        [MaxLength(10)]
        public string StartTime { get; set; }

        // END_TIME
        [MaxLength(10)]
        public string EndTime { get; set; }
    }
}
