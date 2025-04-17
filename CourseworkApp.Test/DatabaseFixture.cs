using Microsoft.EntityFrameworkCore;
using CourseworkApp.Database.Data;
using CourseworkApp.Database.Models;
namespace CourseworkApp.Test;

public class DatabaseFixture
{
  internal TestDbContext? _testDbContext { get; private set; }

  public DatabaseFixture()



  {
    _testDbContext = new TestDbContext();

    _testDbContext.Database.EnsureDeleted();
    _testDbContext.Database.Migrate();
  }

  public void SeedData()
  {
    var mainPageEntry1 = new MainPage()
    {
      Text = "Test Entry"
    };
    _testDbContext.Add(mainPageEntry1);

    var mainPageEntry2 = new MainPage()
    {
      Text = "Test Entry 2"
    };
    _testDbContext.Add(mainPageEntry2);

    var mainPageEntry3 = new MainPage()
    {
      Text = "Test Entry 3"
    };
    _testDbContext.Add(mainPageEntry3);


    _testDbContext.SaveChanges();
  }

}
