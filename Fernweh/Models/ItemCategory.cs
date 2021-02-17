using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Fernweh.Models
{
    public class ItemCategory
    {
        public ItemCategory(string name)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            Items = new List<Item>();
        }

        [JsonIgnore]
        public string Id { get; set; }

        [JsonIgnore]
        public string ParentId { get; set; }

        [JsonIgnore]
        public Trip Parent { get; set; }

        public string Name { get; set; }

        public string Icon { get; set; }

        public List<Item> Items { get; set; }
    }
}