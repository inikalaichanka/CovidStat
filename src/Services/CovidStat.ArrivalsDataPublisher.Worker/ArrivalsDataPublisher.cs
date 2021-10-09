using CovidStat.Infrastructure.MessageBus;
using CovidStat.Services.ArrivalsDataPublisher.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CovidStat.Services.ArrivalsDataPublisher.Worker
{
    public class ArrivalsDataPublisher : BackgroundService
    {
        private readonly ILogger<ArrivalsDataPublisher> _logger;
        private readonly IMessageBus _messageBus;
        private readonly IArrivalRepository _arrivalRepository;

        public ArrivalsDataPublisher(
            ILogger<ArrivalsDataPublisher> logger,
            IMessageBus messageBus,
            IArrivalRepository arrivalRepository)
        {
            _logger = logger;
            _messageBus = messageBus;
            _arrivalRepository = arrivalRepository;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(ArrivalsDataPublisher)} is started at {DateTime.Now}");
            return base.StartAsync(cancellationToken);
        }

        private async Task ProcessMessage(ArrivalViewModel arrival)
        {
            _logger.LogTrace($"{arrival.FullName} arrived to {arrival.City} at {arrival.ArrivalDate.ToShortDateString()}. " +
                $"Departure {(arrival.DepartureDate.HasValue ? $"at {arrival.DepartureDate.Value.ToShortDateString()}" : "is not planned.")}");

            await _arrivalRepository.AddAsync(arrival);
        }

        private Task ProcessError(Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            return Task.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(async () =>
            {
                await _messageBus.DisposeAsync().ConfigureAwait(false);
            });

            if (!stoppingToken.IsCancellationRequested)
            {
                await _messageBus.SubscribeAsync<ArrivalViewModel>(ProcessMessage, ProcessError, stoppingToken).ConfigureAwait(false);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken).ConfigureAwait(false);
            await _messageBus.UnsubscribeAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation($"{nameof(ArrivalsDataPublisher)} is stopped at {DateTime.Now}");
        }
    }
}
