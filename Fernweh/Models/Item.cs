using System;
using System.ComponentModel.DataAnnotations.Schema;
using Fernweh.Data;
using Newtonsoft.Json;

namespace Fernweh.Models
{
    public class Item : IEntityDate
    {
        private bool _packed;

        [JsonIgnore]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonIgnore]
        public string ParentId { get; set; }

        [JsonIgnore]
        public ItemCategory Parent { get; set; }

        public string Name { get; set; }

        public bool Packed
        {
            get => _packed;
            set
            {
                _packed = value;
                _ = DataStore.UpdateItemAsync(this);
            }
        }

        [JsonIgnore] [NotMapped]
        public bool IsEnabled { get; set; } = true;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}