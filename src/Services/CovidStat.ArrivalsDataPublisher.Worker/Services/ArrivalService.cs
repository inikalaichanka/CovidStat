using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CovidStat.Services.ArrivalsDataPublisher.Worker.Services
{
    internal class ArrivalService : IArrivalService
    {
        private readonly HttpClient _httpClient;
        private readonly string _arrivalsUrl;

        public ArrivalService(HttpClient httpClient, IOptions<ArrivalsApiOptions> arrivalsApiOptions)
        {
            _httpClient = httpClient;
            _arrivalsUrl = $"{arrivalsApiOptions.Value.Url}/api/arrivals";
        }

        public async Task AddAsync(ArrivalViewModel arrival)
        {
            var response = await _httpClient.PostAsJsonAsync(_arrivalsUrl, arrival).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }
    }
}
