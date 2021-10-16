using CovidStat.Infrastructure.MessageBus;
using CovidStat.Infrastructure.MessageBusServiceBus;
using CovidStat.Services.ArrivalsDataPublisher.Worker.Services;
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
                        .Configure<ArrivalsApiOptions>(hostContext.Configuration.GetSection(ArrivalsApiOptions.ArrivalsApi));

                    services
                        .AddHostedService<ArrivalsDataPublisher>()
                        .AddSingleton<IArrivalService, ArrivalService>()
                        .AddSingleton<IMessageBus, MessageBusServiceBus>()
                        .AddHttpClient<ArrivalService>();
                })
                .Build();


            await host.RunAsync();
        }
    }
}
