using System.Collections.Generic;

namespace HappyTrip.Model
{
    public class PoiDetail
    {
        public Poi Poi { get; set; }

        public IEnumerable<PoiHistory> History { get; set; }
    }
}
