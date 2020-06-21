using HappyTrip.Model;
using HappyTrip.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HappyTrip.Service.Controllers
{
    [Route("api/[controller]")]
    public class PoiController : Controller
    {
        private static readonly TimeZoneInfo TimeInfo = TimeZoneInfo.CreateCustomTimeZone("ShanghaiTime", new TimeSpan(08, 00, 00), "ShanghaiTime", "ShanghaiTime");
        private readonly PoiContext context;

        public PoiController(PoiContext context)
        {
            this.context = context;
        }

        // GET: api/<controller>
        [HttpGet]
        public async Task<ActionResult<CollectionResult<Poi>>> List()
        {
            var pois = await context.Poi.ToListAsync();
            return new CollectionResult<Poi>()
            {
                Value = pois.ToArray(),
            };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<PoiDetail> Get(int id)
        {
            var poi = await context.Poi.FindAsync(id);
            var historys = await context.PoiHistory
                .Where(p => p.PoiId == id)
                .OrderByDescending(p => p.Date)
                .Take(5)
                .ToListAsync();
            return new PoiDetail
            {
                Poi = poi,
                History = historys.ToArray(),
            };
        }

        [HttpGet("{id}/history/{days}")]
        public async Task<PoiHistory[]> GetHistorys(int id, int days = 5)
        {
            var historys = await context.PoiHistory
                .Where(p => p.PoiId == id)
                .OrderByDescending(p => p.Date)
                .Take(days)
                .ToListAsync();
            return historys.ToArray();
        }
    }
}
