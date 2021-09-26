using System.Threading.Tasks;
using CovidStat.Services.ArrivalsDataProducer.Interfaces;
using CovidStat.Infrastructure.MessageBus;
using CovidStat.Infrastructure.MessageBusServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CovidStat.Services.ArrivalsDataProducer.Worker
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
                        .AddHostedService<ArrivalsDataProducer>()
                        .AddSingleton<IArrivalsDataStorage, ArrivalsDataStorage>()
                        .AddSingleton<IArrivalsDataLoader, ArrivalsDataLoader>()
                        .AddSingleton<IArrivalsHttpClient, ArrivalsHttpClient>()
                        .AddSingleton<IMessageBus, MessageBusServiceBus>()
                        .AddHttpClient<ArrivalsHttpClient>();
                })
                .Build();


            await host.RunAsync();
        }
    }
}
