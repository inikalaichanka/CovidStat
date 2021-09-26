using System;
using System.Threading;
using System.Threading.Tasks;

namespace CovidStat.Infrastructure.MessageBus
{
    public interface IMessageBus : IAsyncDisposable
    {
        Task PublishAsync<T>(T message, CancellationToken cancellationToken = default);

        Task CloseAsync(CancellationToken cancellationToken = default);
    }
}
