using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Fernweh.Services.RestCountries
{
    public class RestCountriesProvider : CacheableProvider
    {
        private const string AllCountriesUrl = "https://restcountries.eu/rest/v2/alpha/";

        public async Task<CountryFacts> GetCountryFactsAsync(string countrycode)
        {
            var json = await GetAsync(AllCountriesUrl + countrycode);
            return JsonConvert.DeserializeObject<CountryFacts>(json);
        }
    }
}