using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseworkApp.Database.Models;

[Table("test")]
[PrimaryKey(nameof(Id))]
public class MainPage
{

  public int Id { get; set; }

  public required string Text { get; set; }

}
