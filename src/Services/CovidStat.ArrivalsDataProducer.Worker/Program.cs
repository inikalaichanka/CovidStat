using System.Threading.Tasks;
using CovidStat.Infrastructure.MessageBus;
using CovidStat.Infrastructure.MessageBusServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CovidStat.Services.ArrivalsDataProducer.Worker.Services;

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
                        .AddSingleton<IArrivalsDataService, ArrivalsDataService>()
                        .AddSingleton<IArrivalsDataLoader, ArrivalsDataLoader>()
                        .AddSingleton<IMessageBus, MessageBusServiceBus>()
                        .AddHttpClient<ArrivalsDataLoader>();
                })
                .Build();


            await host.RunAsync();
        }
    }
}
