using Microsoft.EntityFrameworkCore;
using CourseworkApp.Database.Models;

namespace CourseworkApp.Database.Data
{
    /// <summary>
    /// GenericDbContext is an abstract class that inherits from DbContext.
    /// It provides a base implementation for database contexts in the application.
    /// It contains DbSet properties for various entities and handles the configuration of the database connection.
    /// The OnConfiguring method is responsible for setting up the database connection string.
    /// The OnModelCreating method is used to configure the entity relationships and constraints.
    /// The class also defines two derived classes: CourseDbContext and TestDbContext, which specify different connection names.
    /// </summary>
    public abstract class GenericDbContext : DbContext
    {
        internal abstract string connectionName { get; set; }

        protected GenericDbContext()
        {
        }

        protected GenericDbContext(DbContextOptions options) : base(options)
        {
        }

        // Main Page Tables
        public DbSet<MainPage> MainPageDB { get; set; }

        // Admin Page Tables
        public DbSet<Sensors> SensorsDB { get; set; }
        public DbSet<SensorConfigurations> SensorConfigurationsDB { get; set; }
        public DbSet<FirmwareConfigurations> FirmwareConfigurationsDB { get; set; }
        public DbSet<SensorConfigHistory> SensorConfigHistoryDB { get; set; }
        public DbSet<SensorReadings> SensorReadingsDB { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SensorConfigHistory>(t =>
            {
                t.HasCheckConstraint("CK_SensorConfigHistory_ConfigOrFirmware",
                    @"(ConfigId IS NULL AND FirmwareId IS NOT NULL) OR (ConfigId IS NOT NULL AND FirmwareId IS NULL)");
            });

            modelBuilder.Entity<SensorReadings>(entity =>
            {
                entity.HasOne(reading => reading.Sensor)
                    .WithMany()
                    .HasForeignKey(reading => reading.SensorId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(reading => reading.Config)
                    .WithMany()
                    .HasForeignKey(reading => reading.ConfigId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }
    }

    /// <summary>
    /// TestDbContext is a derived class from GenericDbContext.
    /// It specifies the connection name for the test environment.
    /// </summary>
    public class TestDbContext : GenericDbContext
    {
        internal override string connectionName { get; set; } = "TestConnection";

        public TestDbContext()
        {
        }

        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options)
        {
        }
    }
}
