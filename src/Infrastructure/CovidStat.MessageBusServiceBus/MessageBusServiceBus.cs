using Azure.Messaging.ServiceBus;
using CovidStat.Infrastructure.MessageBus;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CovidStat.Infrastructure.MessageBusServiceBus
{
    public sealed class MessageBusServiceBus : IMessageBus
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly ServiceBusSender _serviceBusSender;

        public MessageBusServiceBus(IOptions<ServiceBusOptions> config)
        {
            _serviceBusClient = new ServiceBusClient(config.Value.ConnectionString);
            _serviceBusSender = _serviceBusClient.CreateSender(config.Value.TopicName);
        }

        public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
        {
            var serviceBusMessage = new ServiceBusMessage(new BinaryData(message, type: typeof(T)));
            await _serviceBusSender
                .SendMessageAsync(serviceBusMessage, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task CloseAsync(CancellationToken cancellationToken = default)
        {
            await _serviceBusSender.CloseAsync(cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask DisposeAsync()
        {
            await _serviceBusSender.DisposeAsync().ConfigureAwait(false);
            await _serviceBusClient.DisposeAsync().ConfigureAwait(false);

            GC.SuppressFinalize(this);
        }
    }
}
