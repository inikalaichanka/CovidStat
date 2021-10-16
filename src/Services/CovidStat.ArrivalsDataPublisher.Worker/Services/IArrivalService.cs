using CovidStat.Services.ArrivalsDataPublisher.Worker.Models;
using System.Threading.Tasks;

namespace CovidStat.Services.ArrivalsDataPublisher.Worker.Services
{
    public interface IArrivalService
    {
        Task AddAsync(ArrivalViewModel arrival);
    }
}
