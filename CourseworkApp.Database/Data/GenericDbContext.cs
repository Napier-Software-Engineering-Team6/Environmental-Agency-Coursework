using Microsoft.EntityFrameworkCore;
using CourseworkApp.Database.Models;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace CourseworkApp.Database.Data
{
    public abstract class GenericDbContext : DbContext
    {
        internal abstract string connectionName { get; set; }

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
    }
}
