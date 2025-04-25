namespace CourseworkApp.Models
{
    public class SensorModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Type { get; set; }
        public double? ThresholdLow { get; set; }
        public double? ThresholdHigh { get; set; }
    }
}
