using System.Threading.Tasks;

namespace CovidStat.Services.ArrivalsDataPublisher.Worker.Services
{
    public interface IArrivalService
    {
        Task AddAsync(ArrivalViewModel arrival);
    }
}
