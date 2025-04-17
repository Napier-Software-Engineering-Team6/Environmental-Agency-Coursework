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

      var a = typeof(GenericDbContext).Assembly;
      var resources = a.GetManifestResourceNames();

      using var stream = a.GetManifestResourceStream("CourseworkApp.Database.appsettings.json");

      var config = new ConfigurationBuilder()
        .AddJsonStream(stream)
        .Build();

      optionsBuilder.UseSqlServer(
        config.GetConnectionString(connectionName),
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