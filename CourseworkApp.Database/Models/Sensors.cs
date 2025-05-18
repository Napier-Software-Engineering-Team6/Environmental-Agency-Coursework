using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseworkApp.Database.Models;

[Table("Sensors")]
[PrimaryKey(nameof(SensorId))]
public class Sensors
{
  public int SensorId { get; set; }
  public required string SensorName { get; set; }
  public required string SensorType { get; set; }
  public required int CurrentConfigId { get; set; }
  [ForeignKey(nameof(CurrentConfigId))]
  public SensorConfigurations? CurrentConfig { get; set; }
  public required int CurrentFirmwareId { get; set; }
  [ForeignKey(nameof(CurrentFirmwareId))]
  public FirmwareConfigurations? CurrentFirmware { get; set; }
}
