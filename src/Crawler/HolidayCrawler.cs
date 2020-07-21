using HappyTrip.Crawler.Model;
using HappyTrip.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HappyTrip.Crawler
{
    public class HolidayCrawler
    {
        static readonly HttpClient client = new HttpClient();
        const int MaxRetry = 3;
        private static ConcurrentDictionary<string, Tuple<HashSet<DateTime>, HashSet<DateTime>>> holidayCache = new ConcurrentDictionary<string, Tuple<HashSet<DateTime>, HashSet<DateTime>>>();

        static HolidayCrawler()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public static async Task<BaiduHolidayModel> GetBaiduHolidayModel(string month)
        {
            int currentRetry = 0;

            while (currentRetry < MaxRetry)
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync($"http://opendata.baidu.com/api.php?query={month}&resource_id=6018&format=json");
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();
                    var model = JsonSerializer.Deserialize<BaiduHolidayModel>(content);
                    return model;
                }
                catch (Exception)
                {
                    currentRetry++;
                    if (currentRetry >= MaxRetry)
                    {
                        throw;
                    }
                }
            }
            return null;
        }

        public async void UpdateDatabase(DateTime date)
        {
            var day = GetHolidayDetail(date.Date);
            var isHoliday = IsHoliday(date.Date);

            TimeZoneInfo timeInfo = TimeZoneInfo.CreateCustomTimeZone("ShanghaiTime", new TimeSpan(08, 00, 00), "ShanghaiTime", "ShanghaiTime");
            string fileName = TimeZoneInfo.ConvertTime(date, timeInfo).ToString("yyyyMMdd");
            int dateInt = Convert.ToInt32(fileName);

            using (var context = new PoiContext())
            {
                var existing = context.DayInformation.SingleOrDefault(p => p.Date == dateInt);
                if (existing != null)
                {
                    existing.HolidayDetail = day;
                    existing.IsHoliday = isHoliday;
                }
                else
                {
                    context.DayInformation.Add(
                        new DayInformation { 
                            Date = dateInt,
                            HolidayDetail = day,
                            IsHoliday = isHoliday,
                        });
                }
                await context.SaveChangesAsync();
            }
        }

        public bool IsHoliday(DateTime date)
        {
            var day = GetHolidayDetail(date);
            if (day == HappyTrip.Model.HolidayDetail.Weekend || day == HappyTrip.Model.HolidayDetail.FreeWorkday)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public HappyTrip.Model.HolidayDetail GetHolidayDetail(DateTime date)
        {
            string monthString = date.ToString(@"yyyy\%E5\%B9\%B4MM\%E6\%9C\%88");
            var daySet = holidayCache.GetOrAdd(monthString, (key) =>
            {
                var model = GetBaiduHolidayModel(key).GetAwaiter().GetResult();
                // daySet = <holidaySet, workdaySet>
                var daySet = BuildHolidaySet(model);
                return daySet;
            });

            if (IsWeekend(date))
            {
                if (daySet.Item2.Contains(date.Date))
                {
                    return HappyTrip.Model.HolidayDetail.WorkWeekend;
                }
                else
                {
                    return HappyTrip.Model.HolidayDetail.Weekend;
                }
            }
            else
            {
                if (daySet.Item1.Contains(date.Date))
                {
                    return HappyTrip.Model.HolidayDetail.FreeWorkday;
                }
                else
                {
                    return HappyTrip.Model.HolidayDetail.Workday;
                }
            }
        }

        public static bool IsWeekend(DateTime date)
        {
            var dayOfWeek = date.DayOfWeek;
            return dayOfWeek == DayOfWeek.Sunday || dayOfWeek == DayOfWeek.Saturday;
        }

        public static Tuple<HashSet<DateTime>, HashSet<DateTime>> BuildHolidaySet(BaiduHolidayModel model)
        {
            var holidaySet = new HashSet<DateTime>();
            var workdaySet = new HashSet<DateTime>();

            var holidayList = model.Data[0].HolidayList;
            if (holidayList != null)
            {
                foreach (var item in holidayList)
                {
                    var dayList = item.DateList;
                    foreach (var day in dayList)
                    {
                        if (day.Status == "1")
                        {
                            holidaySet.Add(DateTime.Parse(day.Date).Date);
                        }
                        else if (day.Status == "2")
                        {
                            workdaySet.Add(DateTime.Parse(day.Date).Date);
                        }
                    }
                }
            }
            return new Tuple<HashSet<DateTime>, HashSet<DateTime>>(holidaySet, workdaySet);
        }
    }
}
