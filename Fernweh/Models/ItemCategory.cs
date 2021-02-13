using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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

        [JsonIgnore] public string Id { get; set; }

        public string Name { get; set; }

        public string Icon { get; set; }

        public List<Item> Items { get; set; }
    }
}