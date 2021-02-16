using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using MonkeyCache.FileStore;
using Xamarin.Essentials;

namespace Fernweh.Services
{
    public abstract class CacheableProvider
    {
        internal async Task<string> GetAsync(string url, int days = 7, bool forceRefresh = false)
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
                Crashes.TrackError(ex);
            }

            return json;
        }
    }
}