using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseworkApp.Database.Models;

[Table("SensorReadings")]
public class SensorReadings
{
  [Key]
  public long ReadingId { get; set; }

  public required int SensorId { get; set; }

  [ForeignKey(nameof(SensorId))]
  public Sensors? Sensor { get; set; }


  public required int ConfigId { get; set; } // FK to SensorConfigurations

  [ForeignKey(nameof(ConfigId))]
  public SensorConfigurations? Config { get; set; } // Navigation property
                                                    // --- END ADD ---

  public required DateTime Timestamp { get; set; } // When the reading occurred

  public required double Value { get; set; } // The actual measured value
}