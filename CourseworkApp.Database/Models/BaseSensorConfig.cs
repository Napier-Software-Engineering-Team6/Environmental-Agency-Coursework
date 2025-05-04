using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CourseworkApp.Database.Models;
public class BaseSensorConfig
{
  public required int MonitorFrequencySeconds { get; set; }
  public required int MonitorDurationSeconds { get; set; }
  public required double LocationLatitude { get; set; }
  public required double LocationLongitude { get; set; }
  public BaseSensorConfig() { }
}

