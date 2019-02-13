using System;

namespace MeteostationService
{
    public class Stats
    {
        public int StatsId { get; set; }
        public string Humidity { get; set; }
        public string Temperature { get; set; }
        public DateTime MeasurementTime { get; set; }
        public ApplicationUser User { get; set; }
    }
}