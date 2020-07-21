using System.Collections.Generic;

namespace HappyTrip.Model
{
    public class DayInformation
    {
        public int Date { get; set; }

        public HolidayDetail HolidayDetail { get; set; }

        public bool IsHoliday { get; set; }
    }
}
