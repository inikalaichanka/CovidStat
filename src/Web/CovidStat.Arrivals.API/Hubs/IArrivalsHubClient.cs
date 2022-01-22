using CovidStat.Web.Arrivals.API.Models;
using System.Threading.Tasks;

namespace CovidStat.Web.Arrivals.API.Hubs
{
    public interface IArrivalsHubClient
    {
        Task ReceiveArrival(ArrivalViewModel arrival);
    }
}
