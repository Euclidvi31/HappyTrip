using HappyTrip.Model;
using HappyTrip.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HappyTrip.Service.Controllers
{
    [Route("api/[controller]")]
    public class PoiController : Controller
    {
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
        public async Task<Poi> Get(int id)
        {
            var poi = await context.Poi.FindAsync(id);
            return poi;
        }
    }
}
