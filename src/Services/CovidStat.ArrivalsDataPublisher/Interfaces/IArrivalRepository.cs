using System.Threading.Tasks;

namespace CovidStat.Services.ArrivalsDataPublisher.Interfaces
{
    public interface IArrivalRepository
    {
        Task AddAsync(ArrivalViewModel arrival);
    }
}
