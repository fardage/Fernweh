using System.Collections.Generic;
using System.Linq;

namespace Fernweh.Services.RestCountries
{
    public class CountryFacts
    {
        public string NativeName { get; set; }
        public int Population { get; set; }
        public List<Language> Languages { get; set; }
        public string MostSpokenLanguage => Languages.FirstOrDefault()?.Name;
        public List<Currency> Currencies { get; set; }
        public string MostUsedCurrency => Currencies.FirstOrDefault()?.Name;
    }
}