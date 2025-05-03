using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseworkApp.Database.Models;

[Table("FirmwareConfigurations")]
[PrimaryKey(nameof(FirmwareId))]
public class FirmwareConfigurations
{

  public int FirmwareId { get; set; }
  public required string SensorType { get; set; }
  public required string FirmwareVersion { get; set; }
  public required string FirmwareData { get; set; }
  public required DateTime ReleaseDate { get; set; }
  public required DateTime EndofLifeDate { get; set; }
  public required bool IsActive { get; set; }
}
