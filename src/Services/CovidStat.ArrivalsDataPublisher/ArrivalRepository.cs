using CovidStat.Services.ArrivalsDataPublisher.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace CovidStat.Services.ArrivalsDataPublisher
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

        public async Task AddAsync(ArrivalViewModel arrival)
        {
            await _arrivals
                .InsertOneAsync(arrival)
                .ConfigureAwait(false);
        }
    }
}
