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
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>();

            // Load appsettings.json
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            builder.Configuration.AddConfiguration(config);

            // Get the connection string safely
            var connectionString = builder.Configuration.GetSection("ConnectionStrings:DevelopmentConnection").Value;

            // Register DbContext with the correct connection string
            builder.Services.AddDbContext<TestDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Dependency Injection for Repositories, Services, ViewModels
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
