using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CourseworkApp.Database.Models;
using System.Reflection;
using System.Text.Json;


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
    internal abstract String connectionName { get; set; }
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
    /// <summary>
    /// OnConfiguring method is overridden to configure the database connection string.
    /// It checks for the connection string in the environment variables and if not found, it looks for it in the appsettings.json file.
    /// If the connection string is not found in either location, it throws an InvalidOperationException.
    /// The connection string is used to connect to the SQL Server database.
    /// The method also specifies the assembly for the migrations.
    /// </summary>
    /// <param name="optionsBuilder"></param>
    /// <exception cref="InvalidOperationException"></exception>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

      var connectionString = Environment.GetEnvironmentVariable($"ConnectionStrings__{connectionName}");

      if (string.IsNullOrEmpty(connectionString))
      {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream("CourseworkApp.Database.appsettings.json");

        if (stream != null)
        {
          var config = new ConfigurationBuilder()
            .AddJsonStream(stream)
            .Build();

          connectionString = config.GetConnectionString(connectionName);
        }
      }

      if (String.IsNullOrEmpty(connectionString))
      {
        throw new InvalidOperationException($"Database connection string '{connectionName}' is not configured.");
      }

      optionsBuilder.UseSqlServer(
          connectionString,
          m => m.MigrationsAssembly("CourseworkApp.Migrations"));
    }
    /// <summary>
    /// OnModelCreating method is overridden to configure the entity relationships and constraints.
    /// It uses the Fluent API to define the relationships between entities and set up the database schema.
    /// The method configures the SensorConfigurations entity to own a JSON property called ConfigData.
    /// It also sets up the relationships between the Sensors and SensorConfigHistory entities.
    /// The Sensors entity has foreign keys to the SensorConfigurations and FirmwareConfigurations entities.
    /// The SensorConfigHistory entity has foreign keys to the SensorConfigurations and FirmwareConfigurations entities.
    /// The method also defines a check constraint on the SensorConfigHistory table to ensure that either ConfigId or FirmwareId is null, but not both.
    /// This ensures that the history records are either for a configuration or a firmware, but not both at the same time.
    /// The method is called when the model for a context is being created.
    /// It is used to configure the model and set up the database schema.
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<SensorConfigurations>()
        .OwnsOne(sc => sc.ConfigData, builder =>
        {
          builder.ToJson();
        });

      modelBuilder.Entity<Sensors>(entity =>
      {
        entity.HasOne(s => s.CurrentConfig)
              .WithMany()
              .HasForeignKey(s => s.CurrentConfigId)
              .IsRequired()
              .OnDelete(DeleteBehavior.NoAction);

        entity.HasOne(s => s.CurrentFirmware)
              .WithMany()
              .HasForeignKey(s => s.CurrentFirmwareId)
              .IsRequired()
              .OnDelete(DeleteBehavior.NoAction);
      });

      modelBuilder.Entity<SensorConfigHistory>(entity =>
      {
        entity.HasOne(h => h.Config)
              .WithMany()
              .HasForeignKey(h => h.ConfigId)
              .IsRequired(false)
              .OnDelete(DeleteBehavior.SetNull);

        entity.HasOne(h => h.Firmware)
              .WithMany()
              .HasForeignKey(h => h.FirmwareId)
              .IsRequired(false)
              .OnDelete(DeleteBehavior.SetNull);

        entity.ToTable("SensorConfigHistory", t =>
        {
          t.HasCheckConstraint("CK_SensorConfigHistory_ConfigOrFirmware",
            @"(ConfigId IS NULL AND FirmwareId IS NOT NULL) OR (ConfigId IS NOT NULL AND FirmwareId IS NULL)");
        });
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
  /// CourseDbContext is a derived class from GenericDbContext.
  /// It specifies the connection name for the development environment.
  /// The connection name is used to retrieve the connection string from the environment variables or appsettings.json file.
  /// </summary>
  public class CourseDbContext : GenericDbContext
  {
    internal override String connectionName { get; set; } = "DevelopmentConnection";
  }
  /// <summary>
  /// TestDbContext is a derived class from GenericDbContext.
  /// It specifies the connection name for the test environment.
  /// The connection name is used to retrieve the connection string from the environment variables or appsettings.json file.
  /// </summary>
  public class TestDbContext : GenericDbContext
  {
    internal override String connectionName { get; set; } = "TestConnection";
  }
}