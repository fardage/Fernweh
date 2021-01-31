using System;
using System.Collections.ObjectModel;

namespace Fernweh.Models
{
    public class Trip
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Destination { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Collection<ItemCategory> Categories { get; set; } = new Collection<ItemCategory>();
        public string ColorA { get; set; }
        public string ColorB { get; set; }
    }
}