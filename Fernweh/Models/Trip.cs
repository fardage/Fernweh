using System;
using System.Collections.ObjectModel;

namespace Fernweh.Models
{
    public class Trip
    {
        public Trip()
        {
            Categories = new Collection<ItemCategory>();
        }

        public Trip(Collection<ItemCategory> categories)
        {
            Id = Guid.NewGuid().ToString();
            Categories = categories;
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Destination { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Collection<ItemCategory> Categories { get; set; }
    }
}