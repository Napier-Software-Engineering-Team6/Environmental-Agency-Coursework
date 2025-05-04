using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseworkApp.Database.Models;

[Table("SensorConfigurations")]
[PrimaryKey(nameof(ConfigId))]
public class SensorConfigurations
{
  public int ConfigId { get; set; }
  public required string SensorType { get; set; }
  public required string ConfigName { get; set; }
  public required BaseSensorConfig ConfigData { get; set; }
  public required DateTime CreatedAt { get; set; }
  public required bool IsActive { get; set; }
}
