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
        private readonly ServiceBusProcessor _serviceBusProcessor;

        public MessageBusServiceBus(IOptions<ServiceBusOptions> config)
        {
            _serviceBusClient = new ServiceBusClient(config.Value.ConnectionString);
            _serviceBusSender = _serviceBusClient.CreateSender(config.Value.TopicName);
            _serviceBusProcessor = _serviceBusClient.CreateProcessor(config.Value.TopicName, config.Value.SubscriptionName);
        }

        public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
        {
            var serviceBusMessage = new ServiceBusMessage(new BinaryData(message, type: typeof(T)));
            await _serviceBusSender
                .SendMessageAsync(serviceBusMessage, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task SubscribeAsync<T>(Func<T, Task> processMessageAction, Func<Exception, Task> processErrorAction = null, CancellationToken cancellationToken = default)
        {
            _serviceBusProcessor.ProcessMessageAsync += async (args) =>
            {
                var messageBody = args.Message.Body.ToObjectFromJson<T>();

                await processMessageAction(messageBody).ConfigureAwait(false);
                await args.CompleteMessageAsync(args.Message, args.CancellationToken).ConfigureAwait(false);
            };

            _serviceBusProcessor.ProcessErrorAsync += async (arg) =>
            {
                if (processErrorAction != null)
                {
                    await processErrorAction(arg.Exception).ConfigureAwait(false);
                }
            };

            await _serviceBusProcessor.StartProcessingAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task UnsubscribeAsync(CancellationToken cancellationToken = default)
        {
            await _serviceBusProcessor.StopProcessingAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task CloseAsync(CancellationToken cancellationToken = default)
        {
            await _serviceBusSender.CloseAsync(cancellationToken).ConfigureAwait(false);
            await _serviceBusProcessor.CloseAsync(cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask DisposeAsync()
        {
            await _serviceBusProcessor.DisposeAsync().ConfigureAwait(false);
            await _serviceBusSender.DisposeAsync().ConfigureAwait(false);
            await _serviceBusClient.DisposeAsync().ConfigureAwait(false);

            GC.SuppressFinalize(this);
        }
    }
}
