using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
    
namespace CourseworkApp.Database.Models;
    
[Table("weather")]
[PrimaryKey(nameof(Id))]
public class WeatherSensor
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    
    
    // Time property for measurement timestamp
    public DateTime Time { get; set; }
    
    // Core weather measurements from your table
    [Column(TypeName = "decimal(5,2)")]
    public decimal Temperature_2m { get; set; }
    
    [Column(TypeName = "decimal(5,2)")]
    public decimal Relative_Humidity_2m { get; set; }
    
    [Column(TypeName = "decimal(5,2)")]
    public decimal Wind_Speed_10m { get; set; }
    
    [Column(TypeName = "decimal(5,2)")]
    public decimal Wind_Direction_10m { get; set; }
    
    // Additional tracking dates
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public List<string> Notes { get; set; } = new List<string>();
}
