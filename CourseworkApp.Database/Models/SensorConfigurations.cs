using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseworkApp.Database.Models;

[Table("SensorConfigurations")]
[PrimaryKey(nameof(ConfigId))]
public class SensorConfigurations
{
  [Required]
  public int ConfigId { get; set; }
  [Required]
  public required string SensorType { get; set; }
  [Required]
  public required string ConfigName { get; set; }
  [Required]
  public required BaseSensorConfig ConfigData { get; set; }
  [Required]
  public required DateTime CreatedAt { get; set; }
  [Required]
  public required bool IsActive { get; set; }
}
