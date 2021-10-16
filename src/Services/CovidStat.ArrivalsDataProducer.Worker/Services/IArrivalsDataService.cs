using CovidStat.Services.ArrivalsDataProducer.Worker.Models;
using System.Threading.Tasks;

namespace CovidStat.Services.ArrivalsDataProducer.Worker.Services
{
    public interface IArrivalsDataService
    {
        Task<ArrivalViewModel> GetNextAsync();
    }
}
