using System;
using System.Threading;
using System.Threading.Tasks;

namespace CovidStat.Infrastructure.MessageBus
{
    public interface IMessageBus : IAsyncDisposable
    {
        Task PublishAsync<T>(T message, CancellationToken cancellationToken = default);

        Task SubscribeAsync<T>(Func<T, Task> processMessageAction, Func<Exception, Task> processErrorAction = null, CancellationToken cancellationToken = default);

        Task UnsubscribeAsync(CancellationToken cancellationToken = default);

        Task CloseAsync(CancellationToken cancellationToken = default);
    }
}
