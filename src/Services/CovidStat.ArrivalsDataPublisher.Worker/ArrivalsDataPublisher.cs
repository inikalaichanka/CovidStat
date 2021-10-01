using CovidStat.Infrastructure.MessageBus;
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

        public ArrivalsDataPublisher(
            ILogger<ArrivalsDataPublisher> logger,
            IMessageBus messageBus)
        {
            _logger = logger;
            _messageBus = messageBus;
        }

        private Task ProcessMessage(ArrivalViewModel arrival)
        {
            _logger.LogInformation($"{arrival.FullName} arrived to {arrival.City} at {arrival.ArrivalDate.ToShortDateString()}. " +
                $"Departure {(arrival.DepartureDate.HasValue ? $"at {arrival.DepartureDate.Value.ToShortDateString()}" : "is not planned.")}");
            return Task.CompletedTask;
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
        }

        private class ArrivalViewModel
        {
            public Guid Id { get; set; }

            public string FullName { get; set; }

            public string Phone { get; set; }

            public string Email { get; set; }

            public DateTime DateOfBirth { get; set; }

            public string Region { get; set; }

            public string City { get; set; }

            public string Address { get; set; }

            public string PostalCode { get; set; }

            public bool IsVaccinated { get; set; }

            public DateTime ArrivalDate { get; set; }

            public DateTime? DepartureDate { get; set; }
        }
    }
}
