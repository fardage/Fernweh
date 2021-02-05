using System.Collections.Generic;

namespace Fernweh.Services.HereMaps
{
    public class HereMapsResponse
    {
        public List<Suggestion> Suggestions { get; set; }
        public List<Item> Items { get; set; }
    }
}