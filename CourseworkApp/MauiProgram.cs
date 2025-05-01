using Microsoft.Extensions.Logging;
using CourseworkApp.Database.Data;
using CourseworkApp.ViewModels;
using CourseworkApp.Views;
using Syncfusion.Maui.Core.Hosting;
using Microsoft.EntityFrameworkCore;
using CourseworkApp.Database.Repositories;
using System;

namespace CourseworkApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });


        builder.Services.AddDbContext<TestDbContext>();
        
        // Add WeatherDbContext
        builder.Services.AddDbContext<WeatherDbContext>(options =>
            options.UseSqlServer("Server=localhost,1433;Database=Environment_data;User Id=sa;Password=Topg3ar10!;TrustServerCertificate=True;",
            sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
                sqlOptions.MigrationsAssembly("CourseworkApp.Migrations");
            }));

        // Register repository and ViewModel
        builder.Services.AddScoped<WeatherRepository>();
        builder.Services.AddSingleton<EnvironmentalScientistViewModel>();

     
     
   // Register the EnvironmentalScientist page
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<MainPageViewModel>();

        builder.Services.AddSingleton<EnvironmentalScientist>();
        builder.Services.AddSingleton<EnvironmentalScientistViewModel>();
        builder.Services.AddScoped<WeatherRepository>();
        builder.Services.AddDbContext<WeatherDbContext>();

        
        
        //--adding in to support graphs for environmental scientist page-->
        builder.ConfigureSyncfusionCore();
    

#if DEBUG

        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
