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
        private const string SharingURL = "http://fernweh.db:3000/api/trips/";
        private readonly HttpClient client;

        public RestService()
        {
            client = new HttpClient();
        }

        public async Task DeleteTripAsync(string id)
        {
            var uri = new Uri(string.Format(SharingURL, id));

            try
            {
                var response = await client.DeleteAsync(uri);

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
            var trip = new Trip();
            var uri = new Uri(SharingURL + id);
            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    trip = JsonConvert.DeserializeObject<Trip>(content);
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
            var uri = new Uri(string.Format(SharingURL, string.Empty));

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
                    response = await client.PostAsync(uri, content);
                else
                    response = await client.PutAsync(uri, content);

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