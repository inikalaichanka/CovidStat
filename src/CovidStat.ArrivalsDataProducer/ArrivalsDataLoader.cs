using CovidStat.ArrivalsDataProducer.Interfaces;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CovidStat.ArrivalsDataProducer
{
    public class ArrivalsDataLoader : IArrivalsDataLoader
    {
        private readonly IArrivalsHttpClient _arrivalsHttpClient;
        private readonly JsonSerializerOptions _deserializeOptions = new() { PropertyNameCaseInsensitive = true };

        public ArrivalsDataLoader(IArrivalsHttpClient arrivalsHttpClient)
        {
            _arrivalsHttpClient = arrivalsHttpClient;
        }

        public async Task<ArrivalViewModel[]> LoadData()
        {
            var response = await _arrivalsHttpClient.FetchData();
            using var contentStream = await response.Content.ReadAsStreamAsync();
            var responseViewModel = await JsonSerializer.DeserializeAsync<ResponseViewModel>(contentStream, _deserializeOptions);

            string serializedData = DecodeContent(responseViewModel.Content);

            var result = JsonSerializer.Deserialize<ArrivalViewModel[]>(serializedData, _deserializeOptions);
            PopulateData(result);

            return result;
        }

        private static string DecodeContent(string content)
        {
            // content comes with special unicode symbols, e.g. 'Wroc\u00c5\u0082aw'
            // it needs to decode them to 'Wrocław'
            var bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(content);
            return Encoding.UTF8.GetString(bytes);
        }

        private static void PopulateData(ArrivalViewModel[] data)
        {
            foreach (var item in data)
            {
                item.DateOfBirth = item.DateOfBirth.Date;
            }
        }

        private class ResponseViewModel
        {
            public bool Success { get; set; }

            public string Content { get; set; }

            public bool IsComplete { get; set; }
        }
    }
}
