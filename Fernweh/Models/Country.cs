using System;

namespace Fernweh.Models
{
    public class Country
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Alpha3Code { get; set; }
        public string Name { get; set; }
        public string NativeName { get; set; }
        public int Population { get; set; }
        public string Flag { get; set; }
    }
}