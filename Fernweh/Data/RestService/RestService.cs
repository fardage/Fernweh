using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Fernweh.Models;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Fernweh.Data.RestService
{
    public class RestService : IRestService
    {
        private const string SharingUrl = "http://fernweh.db:3000/api/trips/";
        private readonly HttpClient _client;

        public RestService()
        {
            _client = new HttpClient();
        }

        public async Task DeleteTripAsync(string id)
        {
            var uri = new Uri(string.Format(SharingUrl));

            try
            {
                var response = await _client.DeleteAsync(uri);

                if (response.IsSuccessStatusCode) Debug.WriteLine(@"\tTodoItem successfully deleted.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                Crashes.TrackError(ex);
            }
        }

        public async Task<Trip> GetTripAsync(string id)
        {
            var uri = new Uri(SharingUrl + id);
            try
            {
                var response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var trip = JsonConvert.DeserializeObject<Trip>(content);
                    return trip;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                Crashes.TrackError(ex);
            }

            return null;
        }

        public async Task SaveTripAsync(Trip trip, bool isNewTrip = false)
        {
            var uri = new Uri(SharingUrl);

            try
            {
                var json = JsonConvert.SerializeObject(trip,
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                if (isNewTrip)
                    response = await _client.PostAsync(uri, content);
                else
                    response = await _client.PutAsync(uri, content);

                if (response.IsSuccessStatusCode) Debug.WriteLine(@"\tTodoItem successfully saved.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                Crashes.TrackError(ex);
            }
        }
    }
}