using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CourseworkApp.Database.Models;
public class BaseSensorConfig
{
  [Required]
  public required int MonitorFrequencySeconds { get; set; }
  [Required]
  public required int MonitorDurationSeconds { get; set; }
  [Required]
  public required double LocationLatitude { get; set; }
  [Required]
  public required double LocationLongitude { get; set; }

  public BaseSensorConfig() { }
}

