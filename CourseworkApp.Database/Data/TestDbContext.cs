using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CourseworkApp.Database.Models;
using System.Reflection;
using System.Text.Json;


namespace CourseworkApp.Database.Data
{
  public abstract class GenericDbContext : DbContext
  {
    internal abstract String connectionName { get; set; }
    public GenericDbContext()
    {
    }

    public GenericDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<MainPage> MainPageDB { get; set; }
    // Admin Page Tables
    public DbSet<Sensors> SensorsDB { get; set; }
    public DbSet<SensorConfigurations> SensorConfigurationsDB { get; set; }
    public DbSet<FirmwareConfigurations> FirmwareConfigurationsDB { get; set; }
    public DbSet<SensorConfigHistory> SensorConfigHistoryDB { get; set; }
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
        entity.HasOne(h => h.Sensor)
              .WithMany()
              .HasForeignKey(h => h.SensorId)
              .OnDelete(DeleteBehavior.Cascade);

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

      base.OnModelCreating(modelBuilder);
    }
  }

  public class CourseDbContext : GenericDbContext
  {
    internal override String connectionName { get; set; } = "DevelopmentConnection";
  }

  public class TestDbContext : GenericDbContext
  {
    internal override String connectionName { get; set; } = "TestConnection";
  }
}