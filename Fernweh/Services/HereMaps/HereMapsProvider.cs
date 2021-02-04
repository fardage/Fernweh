using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using MonkeyCache.FileStore;
using Xamarin.Essentials;

namespace Fernweh.Services
{
    public class HereMapsProvider
    {
        private static readonly string HereAutocompleteUrl =
            "https://autocomplete.geocoder.ls.hereapi.com/6.2/suggest.json?query=";

        private static readonly string HereAutocompleteApiKey = $"&apiKey={Credentials.ApiKeyHereMaps}";

        public static async Task<List<string>> GetAutocomplete(string searchText)
        {
            var url = HereAutocompleteUrl + searchText + HereAutocompleteApiKey;
            var json = await GetAsync(url);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var hereMapsResponse = JsonSerializer.Deserialize<HereMapsResponse>(json, options);
            var suggestionsList = new List<string>();

            foreach (var suggestion in hereMapsResponse.Suggestions) suggestionsList.Add(suggestion.Label);

            return suggestionsList;
        }

        private static async Task<string> GetAsync(string url, int days = 7, bool forceRefresh = false)
        {
            var json = string.Empty;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                json = Barrel.Current.Get<string>(url);

            if (!forceRefresh && !Barrel.Current.IsExpired(url))
                json = Barrel.Current.Get<string>(url);

            try
            {
                if (string.IsNullOrWhiteSpace(json))
                {
                    var client = new HttpClient();
                    json = await client.GetStringAsync(url);
                    Barrel.Current.Add(url, json, TimeSpan.FromDays(days));
                }

                return json;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to get information from server {ex}");
            }

            return json;
        }
    }
}