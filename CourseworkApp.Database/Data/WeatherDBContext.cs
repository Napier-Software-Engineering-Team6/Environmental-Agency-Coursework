using Microsoft.EntityFrameworkCore;
using CourseworkApp.Database.Models;

namespace CourseworkApp.Database.Data
{
    /// <summary>
    /// Developer: Stuart Clarkson
    /// Date: 2025-04-30
    /// Feature: Environmental Scientist - Integrate with Database (#24)
    /// 
    /// This DbContext manages the WeatherSensor entity and its mapping to the database.
    /// It configures the schema, seeds initial data, and supports EF Core migrations.
    /// </summary>
    public class WeatherDbContext : DbContext
    {
        /// <summary>
        /// Represents the WeatherSensor table in the database.
        /// </summary>
        public DbSet<WeatherSensor> Weather { get; set; }

        /// <summary>
        /// Constructor accepting DbContext options (used by DI and design-time factory).
        /// </summary>
        public WeatherDbContext(DbContextOptions<WeatherDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Configures the model schema and seeds initial data for WeatherSensor.
        /// </summary>
        /// <param name="modelBuilder">The model builder used to configure entity mappings.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Always call the base method first to ensure proper EF Core setup.
            base.OnModelCreating(modelBuilder);

            // Configure the WeatherSensor entity's schema and constraints.
            modelBuilder.Entity<WeatherSensor>(entity =>
            {
                // Set the primary key.
                entity.HasKey(e => e.Id);

                // Configure required properties and their constraints.
                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Time)
                      .IsRequired();

                // Specify SQL column types for decimal properties.
                entity.Property(e => e.Temperature_2m)
                      .HasColumnType("decimal(5,2)");

                entity.Property(e => e.Relative_Humidity_2m)
                      .HasColumnType("decimal(5,2)");

                entity.Property(e => e.Wind_Speed_10m)
                      .HasColumnType("decimal(5,2)");

                entity.Property(e => e.Wind_Direction_10m)
                      .HasColumnType("decimal(5,2)");

                // Exclude the Notes property from the database mapping.
                entity.Ignore(e => e.Notes);

                // Seed initial weather data for demonstration and testing.
                entity.HasData(
                    new WeatherSensor
                    {
                        Id = 1,
                        Name = "Default Sensor",
                        Time = DateTime.Now.AddDays(-3),
                        Temperature_2m = 18.5m,
                        Relative_Humidity_2m = 65.0m,
                        Wind_Speed_10m = 12.3m,
                        Wind_Direction_10m = 45.0m,
                        StartDate = DateTime.Now.AddDays(-3),
                        EndDate = DateTime.Now.AddDays(-2)
                    },
                    new WeatherSensor
                    {
                        Id = 2,
                        Name = "Backup Sensor",
                        Time = DateTime.Now.AddDays(-2),
                        Temperature_2m = 20.1m,
                        Relative_Humidity_2m = 70.0m,
                        Wind_Speed_10m = 15.0m,
                        Wind_Direction_10m = 90.0m,
                        StartDate = DateTime.Now.AddDays(-2),
                        EndDate = DateTime.Now.AddDays(-1)
                    },
                     
                    new WeatherSensor
                    {
                        Id = 3,
                        Name = "Default Sensor",
                        Time = DateTime.Now.AddDays(-3),
                        Temperature_2m = 11.5m,
                        Relative_Humidity_2m = 64.0m,
                        Wind_Speed_10m = 11.1m,
                        Wind_Direction_10m = 44.0m,
                        StartDate = DateTime.Now.AddDays(-5),
                        EndDate = DateTime.Now.AddDays(-10)
                    },
                                        new WeatherSensor
                    {
                        Id = 4,
                        Name = "Default Sensor",
                        Time = DateTime.Now.AddDays(-3),
                        Temperature_2m = 15.5m,
                        Relative_Humidity_2m = 65.0m,
                        Wind_Speed_10m = 10.3m,
                        Wind_Direction_10m = 47.0m,
                        StartDate = DateTime.Now.AddDays(-3),
                        EndDate = DateTime.Now.AddDays(-2)
                    },
                                        new WeatherSensor
                    {
                        Id = 5,
                        Name = "Default Sensor",
                        Time = DateTime.Now.AddDays(-7),
                        Temperature_2m = 17.5m,
                        Relative_Humidity_2m = 60.0m,
                        Wind_Speed_10m = 10.3m,
                        Wind_Direction_10m = 45.0m,
                        StartDate = DateTime.Now.AddDays(-1),
                        EndDate = DateTime.Now.AddDays(-1)
                    }
                    
                );
            });
        }
    }
}
