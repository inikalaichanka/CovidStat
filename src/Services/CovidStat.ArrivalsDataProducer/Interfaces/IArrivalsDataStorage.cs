using System.Threading.Tasks;

namespace CovidStat.Services.ArrivalsDataProducer.Interfaces
{
    public interface IArrivalsDataStorage
    {
        Task<ArrivalViewModel> GetNextAsync();
    }
}
