using System.Net.Http;
using System.Threading.Tasks;

namespace CovidStat.Services.ArrivalsDataProducer.Interfaces
{
    public interface IArrivalsHttpClient
    {
        Task<HttpResponseMessage> FetchDataAsync();
    }
}
