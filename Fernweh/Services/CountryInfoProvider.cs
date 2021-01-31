using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Fernweh.Models;
using MonkeyCache.FileStore;
using Xamarin.Essentials;

namespace Fernweh.Services
{
    public static class CountryInfoProvider
    {
        private const string AllCountriesUrl = "https://restcountries.eu/rest/v2/all";

        private const string WorldBankAvgTempUrl =
            "http://climatedataapi.worldbank.org/climateweb/rest/v1/country/mavg/tas/2020/2039/";

        private static readonly HttpClient Client = new HttpClient();

        public static async Task<List<Country>> GetCountriesAsync()
        {
            var json = await GetAsync(AllCountriesUrl);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var countries = JsonSerializer.Deserialize<List<Country>>(json, options);

            return countries;
        }

        public static async Task<Country> GetCountryAsync(string countryName)
        {
            var countries = new List<Country>();
            var json = await GetAsync(AllCountriesUrl);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            countries = JsonSerializer.Deserialize<List<Country>>(json, options);

            return countries.Find(c => c.Name.Equals(countryName));
        }

        public static async Task<Weather> GetAverageWeatherAsync(string alpha3Code)
        {
            var json = await GetAsync(WorldBankAvgTempUrl + alpha3Code + ".json");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var weathers = JsonSerializer.Deserialize<List<Weather>>(json, options);

            return weathers!.FirstOrDefault();
        }

        public static async Task<string> GetAsync(string url, int days = 7, bool forceRefresh = false)
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
                    json = await Client.GetStringAsync(url);
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