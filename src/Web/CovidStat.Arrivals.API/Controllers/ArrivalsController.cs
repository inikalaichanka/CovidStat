using CovidStat.Web.Arrivals.API.Models;
using CovidStat.Web.Arrivals.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CovidStat.Web.Arrivals.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArrivalsController : ControllerBase
    {
        private readonly IArrivalRepository _arrivalRepository;

        public ArrivalsController(IArrivalRepository arrivalRepository)
        {
            _arrivalRepository = arrivalRepository;
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
        }
    }
}
