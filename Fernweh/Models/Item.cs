using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Fernweh.Data;

namespace Fernweh.Models
{
    public class Item
    {
        private bool _packed;

        [JsonIgnore] public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        [JsonIgnore]
        public bool Packed
        {
            get => _packed;
            set
            {
                _packed = value;
                new Task(async () => { await DataStore.UpdateItemAsync(Id, value); }).Start();
            }
        }
    }
}