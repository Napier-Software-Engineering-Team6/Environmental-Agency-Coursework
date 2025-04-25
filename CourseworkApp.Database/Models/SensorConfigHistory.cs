using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CourseworkApp.Database.Models;

[Table("SensorConfigHistory")]
[PrimaryKey(nameof(HistoryId))]
public class SensorConfigHistory
{
  [Required]
  public int HistoryId { get; set; }
  [Required]

  public required int SensorId { get; set; }
  [ForeignKey(nameof(SensorId))]
  public Sensors? Sensor { get; set; }

  public int? ConfigId { get; set; }
  [ForeignKey(nameof(ConfigId))]
  public SensorConfigurations? Config { get; set; }
  public int? FirmwareId { get; set; }
  [Required]
  [ForeignKey(nameof(FirmwareId))]
  public FirmwareConfigurations? Firmware { get; set; }
  public required string ActionType { get; set; }
  [Required]
  public required string Status { get; set; }
  [Required]
  public required string Details { get; set; }
  [Required]
  public required string PerformedBy { get; set; }
  [Required]
  public required DateTime Timestamp { get; set; }
}