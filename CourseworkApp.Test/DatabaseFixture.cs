using CourseworkApp.Database.Data;
using CourseworkApp.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;

namespace CourseworkApp.Test
{
    public class DatabaseFixture
    {
        public CourseDbContext DbContext { get; private set; }

        public DatabaseFixture()
        {
            var options = new DbContextOptionsBuilder<CourseDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            DbContext = new CourseDbContext(options);

            // Seed initial data
            SeedData();
        }

        public void SeedData()
        {
            if (!DbContext.MainPageDB.Any())
            {
                DbContext.MainPageDB.AddRange(
                    new MainPage { Id = 1, Text = "Test MainPage Entry 1" },
                    new MainPage { Id = 2, Text = "Test MainPage Entry 2" }
                );
            }

            if (!DbContext.Sensors.Any())
            {
                DbContext.Sensors.AddRange(
                    new SensorModel { Id = 1, Name = "Sensor A", Location = "Hyrule", Status = "Active", LastUpdated = DateTime.Now, Type = "Air", ThresholdLow = 10, ThresholdHigh = 100 },
                    new SensorModel { Id = 2, Name = "Sensor B", Location = "Raccoon City", Status = "Inactive", LastUpdated = DateTime.Now, Type = "Water", ThresholdLow = 5, ThresholdHigh = 50 }
                );
            }

            DbContext.SaveChanges();
        }
    }
}
