using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CourseworkApp.Database.Models;
using System.Reflection;
using System;


namespace CourseworkApp.Database.Data
{
  public class TestDbContext : DbContext
  {
    public TestDbContext()
    {
    }

    public TestDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<MainPage> MainPageDB { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      // --- PUT THIS CHECK BACK ---
      if (!optionsBuilder.IsConfigured)
      {
        try
        {
          var assembly = Assembly.GetExecutingAssembly();

          // --- USE THE CORRECT JSON FILE NAME ---
          // Verify this name matches your embedded resource
          const string resourceName = "CourseworkApp.Database.appsettings.json";

          using var stream = assembly.GetManifestResourceStream(resourceName);

          // --- ADD NULL CHECK ---
          if (stream == null)
          {
            throw new InvalidOperationException($"Could not find embedded resource '{resourceName}'. Ensure Build Action is EmbeddedResource.");
          }

          var config = new ConfigurationBuilder()
              .AddJsonStream(stream)
              .Build();

          // --- VERIFY THIS NAME matches the key in appsettings.json ---
          const string connectionStringName = "DevelopmentConnection";
          var connectionString = config.GetConnectionString(connectionStringName);

          // --- ADD NULL/EMPTY CHECK ---
          if (string.IsNullOrEmpty(connectionString))
          {
            throw new InvalidOperationException($"Connection string '{connectionStringName}' not found or is empty within the embedded resource '{resourceName}'.");
          }

          // Optional: Add TrustServerCertificate=true if needed
          if (!connectionString.Contains("TrustServerCertificate=", StringComparison.OrdinalIgnoreCase))
          {
            connectionString += ";TrustServerCertificate=true";
          }

          optionsBuilder.UseSqlServer(connectionString, m => m.MigrationsAssembly("CourseworkApp.Migrations"));
          Console.WriteLine($"INFO (OnConfiguring): Configured using embedded resource '{resourceName}'.");
        }
        catch (Exception ex)
        {
          Console.WriteLine($"FATAL ERROR during DbContext.OnConfiguring: {ex}");
          throw; // Rethrow to indicate failure
        }
      }

      // --- PUT THIS BACK ---
      base.OnConfiguring(optionsBuilder);
    }
  }
}