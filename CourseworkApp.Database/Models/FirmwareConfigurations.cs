using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseworkApp.Database.Models;

[Table("FirmwareConfigurations")]
[PrimaryKey(nameof(FirmwareId))]
public class FirmwareConfigurations
{
  [Required]
  public int FirmwareId { get; set; }
  [Required]
  public required string SensorType { get; set; }
  [Required]
  public required string FirmwareVersion { get; set; }
  [Required]
  public required string FirmwareData { get; set; }
  [Required]
  public required DateTime ReleaseDate { get; set; }
  [Required]
  public required DateTime EndofLifeDate { get; set; }
  [Required]
  public required bool IsActive { get; set; }
}
