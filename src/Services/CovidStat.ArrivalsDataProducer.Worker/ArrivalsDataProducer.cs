using System;
using System.Threading;
using System.Threading.Tasks;
using CovidStat.Services.ArrivalsDataProducer.Interfaces;
using CovidStat.Infrastructure.MessageBus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CovidStat.Services.ArrivalsDataProducer.Worker
{
    public class ArrivalsDataProducer : BackgroundService
    {
        private readonly ILogger<ArrivalsDataProducer> _logger;
        private readonly IArrivalsDataStorage _arrivalsDataStorage;
        private readonly IMessageBus _messageBus;
        private readonly Random _random = new();

        public ArrivalsDataProducer(
            ILogger<ArrivalsDataProducer> logger,
            IArrivalsDataStorage arrivalsDataStorage,
            IMessageBus messageBus)
        {
            _logger = logger;
            _arrivalsDataStorage = arrivalsDataStorage;
            _messageBus = messageBus;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(ArrivalsDataProducer)} is started at {DateTime.Now}");
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(async () =>
            {
                await _messageBus.DisposeAsync().ConfigureAwait(false);
            });

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    ArrivalViewModel arrival = await ProduceNextAsync();
                    _logger.LogTrace($"{arrival.FullName} goes to {arrival.City} at: {DateTimeOffset.Now}.");

                    await _messageBus.PublishAsync(arrival, stoppingToken);

                    await Task.Delay(_random.Next(10000, 30000), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _messageBus.CloseAsync(cancellationToken).ConfigureAwait(false);
            await base.StopAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation($"{nameof(ArrivalsDataProducer)} is stopped at {DateTime.Now}");
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
