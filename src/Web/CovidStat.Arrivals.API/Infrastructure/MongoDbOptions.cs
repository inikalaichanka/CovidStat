namespace CovidStat.Web.Arrivals.API.Infrastructure
{
    public class MongoDbOptions
    {
        public const string MongoDb = "MongoDb";

        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public string CollectionName { get; set; }
    }
}
