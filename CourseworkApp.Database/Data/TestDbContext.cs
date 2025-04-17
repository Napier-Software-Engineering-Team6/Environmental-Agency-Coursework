using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CourseworkApp.Database.Models;
using System.Reflection;
using System;
using System.Diagnostics;


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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

      var connectionString = Environment.GetEnvironmentVariable($"ConnectionStrings:{connectionName}");

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