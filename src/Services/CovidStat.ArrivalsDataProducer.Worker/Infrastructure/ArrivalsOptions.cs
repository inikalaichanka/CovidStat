namespace CovidStat.Services.ArrivalsDataProducer.Worker.Infrastructure
{
    public class ArrivalsOptions
    {
        public const string Arrivals = "Arrivals";

        public int NextArrivalFrequencyMin { get; set; }

        public int NextArrivalFrequencyMax { get; set; }
    }
}
