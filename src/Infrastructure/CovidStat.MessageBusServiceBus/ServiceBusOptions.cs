namespace CovidStat.Infrastructure.MessageBusServiceBus
{
    public class ServiceBusOptions
    {
        public const string ServiceBus = "ServiceBus";

        public string ConnectionString { get; set; }

        public string TopicName { get; set; }
    }
}
