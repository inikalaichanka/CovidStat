using CovidStat.Services.ArrivalsDataProducer.Worker.Infrastructure;
using CovidStat.Services.ArrivalsDataProducer.Worker.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CovidStat.Services.ArrivalsDataProducer.Worker.Services
{
    public class ArrivalsDataLoader : IArrivalsDataLoader
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _deserializeOptions = new() { PropertyNameCaseInsensitive = true };
        private const string RequestUri = "https://generatedata3.com/ajax.php";

        public ArrivalsDataLoader(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ArrivalViewModel[]> LoadDataAsync()
        {
            var response = await FetchDataAsync();
            using var contentStream = await response.Content.ReadAsStreamAsync();
            var responseViewModel = await JsonSerializer.DeserializeAsync<ResponseViewModel>(contentStream, _deserializeOptions);

            string serializedData = DecodeContent(responseViewModel.Content);

            var result = JsonSerializer.Deserialize<ArrivalViewModel[]>(serializedData, _deserializeOptions);
            PopulateData(result);

            return result;
        }

        private async Task<HttpResponseMessage> FetchDataAsync() => await _httpClient.PostAsync(RequestUri, GetRequestContent());

        private StringContent GetRequestContent()
        {
            var requestData = new Dictionary<string, string>
            {
                ["gdRowOrder"] = "1,2,3,4,5,6,7,8,9,10",
                ["gdExportType"] = "JSON",
                ["gdNumCols"] = "10",
                ["gdCountries[]"] = "poland",
                ["gdTitle_1"] = "id",
                ["gdDataType_1"] = "data-type-GUID",
                ["gdTitle_2"] = "fullName",
                ["gdDataType_2"] = "data-type-Names",
                ["dtOption_2"] = "Name+Surname",
                ["gdTitle_3"] = "phone",
                ["gdDataType_3"] = "data-type-Phone",
                ["dtOption_3"] = "1-Xxx-Xxx-xxxx|Xxx-xxxx",
                ["gdTitle_4"] = "email",
                ["gdDataType_4"] = "data-type-Email",
                ["gdTitle_5"] = "dateOfBirth",
                ["gdDataType_5"] = "data-type-Date",
                ["dtFromDate_5"] = "01/01/1946",
                ["dtToDate_5"] = "01/01/2020",
                ["dtOption_5"] = "c",
                ["gdTitle_6"] = "region",
                ["gdDataType_6"] = "data-type-Region",
                ["dtIncludeRegion_poland_6"] = "on",
                ["dtIncludeRegion_poland_Full_6"] = "on",
                ["gdTitle_7"] = "city",
                ["gdDataType_7"] = "data-type-City",
                ["gdTitle_8"] = "address",
                ["gdDataType_8"] = "data-type-StreetAddress",
                ["gdTitle_9"] = "postalCode",
                ["gdDataType_9"] = "data-type-PostalZip",
                ["dtCountryIncludeZip_poland_9"] = "on",
                ["gdTitle_10"] = "isVaccinated",
                ["gdDataType_10"] = "data-type-Boolean",
                ["dtOption_10"] = "false|true",
                ["etJSON_stripWhitespace"] = "1",
                ["etJSON_dataStructureFormat"] = "simple",
                ["gdNumRowsToGenerate"] = "100",
                ["action"] = "generateInPage",
                ["gdBatchSize"] = Constants.FetchDataBatchSize.ToString(),
                ["gdCurrentBatchNum"] = "1"
            };


            string content = string.Join('&', requestData.Select(x => $"{x.Key}={x.Value}"));
            return new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");
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
