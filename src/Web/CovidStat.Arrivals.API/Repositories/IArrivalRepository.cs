using CovidStat.Web.Arrivals.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CovidStat.Web.Arrivals.API.Repositories
{
    public interface IArrivalRepository
    {
        Task<IEnumerable<ArrivalViewModel>> GetAsync();
    }
}
