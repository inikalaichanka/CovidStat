using CovidStat.Infrastructure.MessageBus;
using CovidStat.Infrastructure.MessageBusServiceBus;
using CovidStat.Services.ArrivalsDataPublisher.Interfaces;
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
                        .Configure<ServiceBusOptions>(hostContext.Configuration.GetSection(ServiceBusOptions.ServiceBus))
                        .Configure<MongoDbOptions>(hostContext.Configuration.GetSection(MongoDbOptions.MongoDb));

                    services
                        .AddHostedService<ArrivalsDataPublisher>()
                        .AddSingleton<IArrivalRepository, ArrivalRepository>()
                        .AddSingleton<IMessageBus, MessageBusServiceBus>();
                })
                .Build();


            await host.RunAsync();
        }
    }
}
