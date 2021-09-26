using System.Threading.Tasks;
using CovidStat.ArrivalsDataProducer.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CovidStat.ArrivalsDataProducer.Worker
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services
                        .AddHostedService<ArrivalsDataProducer>()
                        .AddSingleton<IArrivalsDataStorage, ArrivalsDataStorage>()
                        .AddSingleton<IArrivalsDataLoader, ArrivalsDataLoader>()
                        .AddSingleton<IArrivalsHttpClient, ArrivalsHttpClient>()
                        .AddHttpClient<ArrivalsHttpClient>();
                })
                .Build();


            await host.RunAsync();
        }
    }
}
