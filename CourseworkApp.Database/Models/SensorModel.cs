using CourseworkApp.Models.Enums;
using System;

namespace CourseworkApp.Database.Models
{
    public class SensorModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public SensorStatus Status { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Type { get; set; }
        public double? ThresholdLow { get; set; }
        public double? ThresholdHigh { get; set; }
    }
}