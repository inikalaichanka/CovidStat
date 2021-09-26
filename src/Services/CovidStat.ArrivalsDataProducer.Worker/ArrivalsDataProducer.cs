using System;
using System.Threading;
using System.Threading.Tasks;
using CovidStat.Services.ArrivalsDataProducer.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CovidStat.Services.ArrivalsDataProducer.Worker
{
    public class ArrivalsDataProducer : BackgroundService
    {
        private readonly ILogger<ArrivalsDataProducer> _logger;
        private readonly IArrivalsDataStorage _arrivalsDataStorage;
        private readonly Random _random = new();

        public ArrivalsDataProducer(ILogger<ArrivalsDataProducer> logger, IArrivalsDataStorage arrivalsDataStorage)
        {
            _logger = logger;
            _arrivalsDataStorage = arrivalsDataStorage;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(ArrivalsDataProducer)} running at: {DateTimeOffset.Now}");
                try
                {
                    ArrivalViewModel arrival = await ProduceNextAsync();
                    _logger.LogInformation($"{arrival.FullName} arrived to {arrival.City} at {arrival.ArrivalDate.ToShortDateString()}. " +
                        $"Departure {(arrival.DepartureDate.HasValue ? $"at {arrival.DepartureDate.Value.ToShortDateString()}" : "is not planned.")}");

                    await Task.Delay(_random.Next(10000, 30000), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        private async Task<ArrivalViewModel> ProduceNextAsync()
        {
            ArrivalViewModel arrival = await _arrivalsDataStorage.GetNextAsync();
            arrival.ArrivalDate = DateTime.Now.Date;

            if (_random.Next(0, 10) != 9)
            {
                int departureDays = _random.Next(1, 90);
                arrival.DepartureDate = arrival.ArrivalDate.AddDays(departureDays).Date;
            }

            return arrival;
        }
    }
}
