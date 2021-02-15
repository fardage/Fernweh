using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Fernweh.Models
{
    public class Trip
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Destination { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<ItemCategory> Categories { get; set; } = new List<ItemCategory>();
        public string ColorA { get; set; }
        public string ColorB { get; set; }
        [JsonIgnore]
        public bool IsShared { get; set; }
    }
}