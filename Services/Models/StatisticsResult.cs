using System;

namespace MeteostationService
{
    public class StatisticsResult
    {
        public string Temperature { get; set; }
        public string Humidity { get; set; }
        public DateTime MeasurementTime { get; set; }
    }
}