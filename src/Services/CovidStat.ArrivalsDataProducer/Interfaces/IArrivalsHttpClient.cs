using System.Net.Http;
using System.Threading.Tasks;

namespace CovidStat.ArrivalsDataProducer.Interfaces
{
    public interface IArrivalsHttpClient
    {
        Task<HttpResponseMessage> FetchDataAsync();
    }
}
