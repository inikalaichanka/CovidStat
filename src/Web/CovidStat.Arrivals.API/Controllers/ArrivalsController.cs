using CovidStat.Web.Arrivals.API.Hubs;
using CovidStat.Web.Arrivals.API.Models;
using CovidStat.Web.Arrivals.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CovidStat.Web.Arrivals.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArrivalsController : ControllerBase
    {
        private readonly IArrivalRepository _arrivalRepository;
        private readonly IHubContext<ArrivalsHub, IArrivalsHubClient> _hubContext;

        public ArrivalsController(
            IArrivalRepository arrivalRepository,
            IHubContext<ArrivalsHub, IArrivalsHubClient> hubContext)
        {
            _arrivalRepository = arrivalRepository;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IEnumerable<ArrivalViewModel>> GetAsync()
        {
            return await _arrivalRepository.GetAsync();
        }

        [Route("latest")]
        [HttpGet]
        public async Task<IEnumerable<ArrivalViewModel>> GetLatestAsync(int count)
        {
            return await _arrivalRepository.GetLatestAsync(count);
        }

        [HttpPost]
        public async Task AddAsync(ArrivalViewModel arrival)
        {
            await _arrivalRepository.AddAsync(arrival);
            await _hubContext.Clients.All.ReceiveArrival(arrival);
        }
    }
}
