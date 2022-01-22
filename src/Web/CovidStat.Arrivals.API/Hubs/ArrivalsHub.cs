using Microsoft.AspNetCore.SignalR;

namespace CovidStat.Web.Arrivals.API.Hubs
{
    public class ArrivalsHub : Hub<IArrivalsHubClient>
    {
    }
}
