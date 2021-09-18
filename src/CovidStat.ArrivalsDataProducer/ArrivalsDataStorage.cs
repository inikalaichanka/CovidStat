using CovidStat.ArrivalsDataProducer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidStat.ArrivalsDataProducer
{
    public class ArrivalsDataStorage : IArrivalsDataStorage
    {
        private readonly Dictionary<Guid, ArrivalViewModel> _cache = new(Constants.FetchDataBatchSize);
        private readonly IArrivalsDataLoader _arrivalsDataLoader;

        public ArrivalsDataStorage(IArrivalsDataLoader arrivalsDataLoader) => _arrivalsDataLoader = arrivalsDataLoader;

        public async Task<ArrivalViewModel> GetNext()
        {
            if (_cache.Count == 0)
            {
                await PopulateCache();
            }

            return PopFromCache();
        }

        private async Task PopulateCache()
        {
            ArrivalViewModel[] data = await _arrivalsDataLoader.LoadData();
            foreach (var item in data)
            {
                _cache.Add(item.Id, item);
            }
        }

        private ArrivalViewModel PopFromCache()
        {
            var item = _cache.First();
            _cache.Remove(item.Key);

            return item.Value;
        }
    }
}
