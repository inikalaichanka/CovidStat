using CovidStat.Infrastructure.MessageBus;
using CovidStat.Infrastructure.MessageBusServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace CovidStat.Services.ArrivalsDataPublisher.Worker
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services
                        .Configure<ServiceBusOptions>(hostContext.Configuration.GetSection(ServiceBusOptions.ServiceBus));

                    services
                        .AddHostedService<ArrivalsDataPublisher>()
                        .AddSingleton<IMessageBus, MessageBusServiceBus>();
                })
                .Build();


            await host.RunAsync();
        }
    }
}
