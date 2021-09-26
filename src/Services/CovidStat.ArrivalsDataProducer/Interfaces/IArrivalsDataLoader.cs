using System.Threading.Tasks;

namespace CovidStat.Services.ArrivalsDataProducer.Interfaces
{
    public interface IArrivalsDataLoader
    {
        Task<ArrivalViewModel[]> LoadDataAsync();
    }
}
