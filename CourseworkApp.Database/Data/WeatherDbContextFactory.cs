using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CourseworkApp.Database.Data
{
    /// <summary>
    /// Design-time factory for WeatherDbContext.
    /// 
    /// This class allows Entity Framework Core tools (such as migrations and database updates)
    /// to create an instance of WeatherDbContext at design time, especially when the context 
    /// is in a class library or requires constructor parameters.
    /// 
    /// The factory implements IDesignTimeDbContextFactory and provides the connection string
    /// and options needed to construct the context outside of the main application runtime.
    /// </summary>
    public class WeatherDbContextFactory : IDesignTimeDbContextFactory<WeatherDbContext>
    {
        /// <summary>
        /// Creates a new instance of WeatherDbContext with the required options.
        /// 
        /// This method is called by EF Core CLI tools at design time. It configures the context
        /// to use SQL Server with the specified connection string.
        /// </summary>
        /// <param name="args">Command-line arguments (not used here).</param>
        /// <returns>A configured WeatherDbContext instance.</returns>
        public WeatherDbContext CreateDbContext(string[] args)
        {
            // Build the options for WeatherDbContext, including the SQL Server connection string.
            var optionsBuilder = new DbContextOptionsBuilder<WeatherDbContext>();
            optionsBuilder.UseSqlServer(
                "Server=localhost,1433;Database=Environment_data;User Id=sa;Password=Topg3ar10!;TrustServerCertificate=True;");

            // Return a new WeatherDbContext instance with these options.
            return new WeatherDbContext(optionsBuilder.Options);
        }
    }
}
