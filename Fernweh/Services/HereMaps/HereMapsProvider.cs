using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Fernweh.Services.HereMaps
{
    public class HereMapsProvider : ApiProvider
    {
        private const string HereAutocompleteUrl =
            "https://autocomplete.geocoder.ls.hereapi.com/6.2/suggest.json?&language=en&query=";

        private const string HereGeocode =
            "https://geocode.search.hereapi.com/v1/geocode?q=";

        private readonly string _hereAutocompleteApiKey = $"&apiKey={Credentials.ApiKeyHereMaps}";

        public async Task<List<Suggestion>> GetAutocomplete(string searchText)
        {
            var url = HereAutocompleteUrl + searchText + _hereAutocompleteApiKey;
            var json = await GetAsync(url);

            var hereMapsResponse = JsonConvert.DeserializeObject<HereMapsResponse>(json);

            return hereMapsResponse.Suggestions;
        }

        public async Task<Address> GetGeocode(string searchText)
        {
            var url = HereGeocode + searchText + _hereAutocompleteApiKey;
            var json = await GetAsync(url);

            var hereMapsResponse = JsonConvert.DeserializeObject<HereMapsResponse>(json);

            return hereMapsResponse.Items.FirstOrDefault().Address;
        }
    }
}