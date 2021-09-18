using System.Threading.Tasks;

namespace CovidStat.ArrivalsDataProducer.Interfaces
{
    public interface IArrivalsDataLoader
    {
        Task<ArrivalViewModel[]> LoadData();
    }
}
