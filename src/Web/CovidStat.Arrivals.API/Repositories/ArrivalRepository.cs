using CovidStat.Web.Arrivals.API.Infrastructure;
using CovidStat.Web.Arrivals.API.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CovidStat.Web.Arrivals.API.Repositories
{
    public class ArrivalRepository : IArrivalRepository
    {
        private readonly IMongoCollection<ArrivalViewModel> _arrivals;

        public ArrivalRepository(IOptions<MongoDbOptions> databaseOptions)
        {
            _arrivals = new MongoClient(databaseOptions.Value.ConnectionString)
                .GetDatabase(databaseOptions.Value.DatabaseName)
                .GetCollection<ArrivalViewModel>(databaseOptions.Value.CollectionName);
        }

        public async Task<IEnumerable<ArrivalViewModel>> GetAsync()
        {
            return await _arrivals
                .Find(FilterDefinition<ArrivalViewModel>.Empty)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task AddAsync(ArrivalViewModel arrival)
        {
            await _arrivals
                .InsertOneAsync(arrival)
                .ConfigureAwait(false);
        }
    }
}
