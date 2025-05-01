using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using CourseworkApp.Database.Data;
using CourseworkApp.Repositories;
using CourseworkApp.Services;
using CourseworkApp.ViewModels;
using CourseworkApp.Views;

namespace CourseworkApp
{
    public static class MauiProgram
    {
        /// <summary>
        /// Configures and builds the MAUI application with required services and dependencies.
        /// </summary>
        /// <returns>The fully configured MAUI app.</returns>
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            // Register the main application class
            builder.UseMauiApp<App>();

            // Load configuration from appsettings.json
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            builder.Configuration.AddConfiguration(config);

            // Retrieve the connection string for the development database
            var connectionString = builder.Configuration
                .GetSection("ConnectionStrings:DevelopmentConnection").Value;

            // ✅ Register CourseDbContext with the connection string
            builder.Services.AddDbContext<CourseDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Register application services and dependencies for DI
            builder.Services.AddTransient<ISensorRepository, SensorRepository>();
            builder.Services.AddTransient<SensorService>();
            builder.Services.AddTransient<SensorViewModel>();
            builder.Services.AddTransient<SensorPage>();
            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<MainPage>();

            return builder.Build();
        }
    }
}
