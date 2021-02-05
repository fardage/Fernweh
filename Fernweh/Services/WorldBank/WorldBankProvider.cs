using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Fernweh.Services.WorldBank
{
    public class WorldBankProvider : ApiProvider
    {
        private const string WorldBankAvgTempUrl =
            "http://climatedataapi.worldbank.org/climateweb/rest/v1/country/mavg/tas/2020/2039/";

        public async Task<Weather> GetAverageWeatherAsync(string alpha3Code)
        {
            var json = await GetAsync(WorldBankAvgTempUrl + alpha3Code + ".json");
            var weathers = JsonConvert.DeserializeObject<List<Weather>>(json);

            return weathers!.FirstOrDefault();
        }
    }
}