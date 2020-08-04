﻿using HappyTrip.Model;
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
            var pois = await context.Poi.OrderByDescending(p => p.TrafficNumber).ToListAsync();
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
                .Take(7)
                .ToListAsync();
            historys.Reverse();
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
            historys.Reverse();
            return historys.ToArray();
        }

        private bool FilterWithPoiId(int id)
        {
            return id == 2  // 上海野生动物园
                || id == 8  // 上海影视乐园
                || id == 13 // 上海和平公园
                || id == 14 // 上海鲁迅公园
                || id == 16 // 上海田子坊景区
                || id == 26 // 上海国际旅游度假区
                || id == 29 // 浦江郊野公园
                || id == 42 // 上海中心
                || id == 54 // 上海海洋水族馆
                || id == 57 // 锦江乐园
                || id == 64 // 上海动物园
                || id == 71 // 上海博物馆
                || id == 72 // 上海自然博物馆（上海科技馆分馆）
                || id == 73 // 上海科技馆
                || id == 77 // 上海长风公园
                || id == 79 // 上海杜莎夫人蜡像馆
                ;
        }
    }
}
