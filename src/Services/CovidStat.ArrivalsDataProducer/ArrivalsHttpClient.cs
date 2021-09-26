using CovidStat.Services.ArrivalsDataProducer.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CovidStat.Services.ArrivalsDataProducer
{
    public class ArrivalsHttpClient : IArrivalsHttpClient
    {
        private readonly HttpClient _httpClient;
        private const string RequestUri = "https://generatedata3.com/ajax.php";

        public ArrivalsHttpClient(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<HttpResponseMessage> FetchDataAsync() => await _httpClient.PostAsync(RequestUri, GetRequestContent());

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
    }
}
