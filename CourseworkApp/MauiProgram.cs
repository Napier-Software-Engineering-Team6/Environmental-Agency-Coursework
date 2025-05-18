using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using CourseworkApp.Database.Data;
using CourseworkApp.Repositories;
using CourseworkApp.Services;
using CourseworkApp.ViewModels;
using CourseworkApp.Views;
using CourseworkApp.Services.Factory;
using Syncfusion.Maui.Core.Hosting;

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

        builder.Services.AddDbContextFactory<TestDbContext>();

        // Load Configuration
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        builder.Configuration.AddConfiguration(config);

        var connectionString = builder.Configuration
            .GetSection("ConnectionStrings:DevelopmentConnection").Value;

        builder.Services.AddDbContext<CourseDbContext>(options =>
            options.UseSqlServer(connectionString));

        // View + ViewModel registration
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddTransient<MainPageViewModel>();
        builder.Services.AddTransient<MainPage>();

        builder.Services.AddSingleton<AdminConfig>();
        builder.Services.AddSingleton<AdminConfigViewModel>();
        builder.Services.AddSingleton<ConfigForm>();
        builder.Services.AddSingleton<ConfigFormViewModel>();
        builder.Services.AddSingleton<AdminFirmware>();
        builder.Services.AddSingleton<AdminFirmwareViewModel>();
        builder.Services.AddSingleton<FirmwareFormViewModel>();
        builder.Services.AddSingleton<FirmwareForm>();

        builder.Services.AddTransient<SensorViewModel>();
        builder.Services.AddTransient<SensorPage>();

        // Services
        builder.Services.AddSingleton<ISensorConfigurationFactory, SensorConfigurationFactory>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<IValidationService, ValidationService>();
        builder.Services.AddSingleton<IConfigurationService, ConfigurationService>();
        builder.Services.AddSingleton<ILoggingService, LoggingService>();
        builder.Services.AddSingleton<ISensorHistoryService, SensorHistoryService>();
        builder.Services.AddSingleton<ILoginService, LoginService>();
        builder.Services.AddSingleton<IFirmwareService, FirmwareService>();
        builder.Services.AddTransient<ISensorRepository, SensorRepository>();
        builder.Services.AddSingleton<IAlertService, AlertService>();
        builder.Services.AddTransient<SensorService>();
        builder.Services.AddTransient<SensorViewModel>();

        // Register Syncfusion Core
        builder.ConfigureSyncfusionCore();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}