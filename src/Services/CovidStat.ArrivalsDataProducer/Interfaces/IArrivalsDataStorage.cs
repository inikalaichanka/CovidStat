using System.Threading.Tasks;

namespace CovidStat.ArrivalsDataProducer.Interfaces
{
    public interface IArrivalsDataStorage
    {
        Task<ArrivalViewModel> GetNext();
    }
}
