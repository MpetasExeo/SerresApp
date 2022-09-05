using System;
using System.Collections.Generic;

namespace SerresApp.Models
{
    public class POI : POISlim
    {
        public bool IsFavorite { get; set; }
        public Temperatures Temperatures { get; set; }
        public List<CategorySlim> Tags { get; set; }
        public List<Uri> Images { get; set; }
    }
}
