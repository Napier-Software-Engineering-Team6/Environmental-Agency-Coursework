using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseworkApp.Database.Models;

[Table("Sensors")]
[PrimaryKey(nameof(SensorId))]
public class Sensors
{
  [Required]
  public int SensorId { get; set; }
  [Required]
  public required string SensorName { get; set; }
  [Required]
  public required string SensorType { get; set; }
  [Required]
  public required int CurrentConfigId { get; set; }
  [ForeignKey(nameof(CurrentConfigId))]
  public SensorConfigurations? CurrentConfig { get; set; }
  [Required]
  public required int CurrentFirmwareId { get; set; }
  [ForeignKey(nameof(CurrentFirmwareId))]
  public FirmwareConfigurations? CurrentFirmware { get; set; }
}
