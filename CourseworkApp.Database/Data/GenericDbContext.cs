using Microsoft.EntityFrameworkCore;
using CourseworkApp.Database.Models;
using CourseworkApp.Models.Enums;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace CourseworkApp.Database.Data
{
    public abstract class GenericDbContext : DbContext
    {
        internal abstract string connectionName { get; set; }

        public DbSet<SensorConfigurations> SensorConfigurationsDB { get; set; }
        public DbSet<FirmwareConfigurations> FirmwareConfigurationsDB { get; set; }
        public DbSet<SensorConfigHistory> SensorConfigHistoryDB { get; set; }

        public GenericDbContext()
        {
        }

        public GenericDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<MainPage> MainPageDB { get; set; }
        public DbSet<SensorModel> Sensors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
                return;

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

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException($"Database connection string '{connectionName}' is not configured.");
            }

            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Store SensorStatus enum as strings in the database
            modelBuilder.Entity<SensorModel>()
                .Property(s => s.Status)
                .HasConversion<string>();

            // Instruct EF to ignore the base class that's not mapped to DB
            modelBuilder.Ignore<BaseSensorConfig>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
