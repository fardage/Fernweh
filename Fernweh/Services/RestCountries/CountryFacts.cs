using System;

namespace Fernweh.Services.RestCountries
{
    public class CountryFacts
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Cioc { get; set; }
        public string Name { get; set; }
        public string NativeName { get; set; }
        public int Population { get; set; }
        public string Flag { get; set; }
    }
}