using Microsoft.Extensions.Logging;
using CourseworkApp.Database.Data;
using CourseworkApp.ViewModels;
using CourseworkApp.Views;
using CourseworkApp.Services;
using CourseworkApp.Services.Factory;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Controls.Xaml;
using Syncfusion.Maui.Core.Hosting;



namespace CourseworkApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();

		builder
				.UseMauiApp<App>()
				.ConfigureSyncfusionCore()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				});


		builder.Services.AddDbContextFactory<TestDbContext>();


		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<MainPageViewModel>();
		builder.Services.AddTransient<AdminConfig>();
		builder.Services.AddTransient<AdminConfigViewModel>();
		builder.Services.AddTransient<ConfigForm>();
		builder.Services.AddTransient<ConfigFormViewModel>();
		builder.Services.AddTransient<AdminFirmware>();
		builder.Services.AddTransient<AdminFirmwareViewModel>();
		builder.Services.AddTransient<FirmwareFormViewModel>();
		builder.Services.AddTransient<FirmwareForm>();
		builder.Services.AddTransient<EnvironmentalScientist>();
		builder.Services.AddTransient<EnvironmentalScientistViewModel>();
		builder.Services.AddTransient<SensorMap>();
		builder.Services.AddTransient<SensorMapViewModel>();


		builder.Services.AddSingleton<ISensorConfigurationFactory, SensorConfigurationFactory>();
		builder.Services.AddSingleton<INavigationService, NavigationService>();
		builder.Services.AddSingleton<IValidationService, ValidationService>();
		builder.Services.AddSingleton<IConfigurationService, ConfigurationService>();
		builder.Services.AddSingleton<ILoggingService, LoggingService>();
		builder.Services.AddSingleton<ISensorHistoryService, SensorHistoryService>();
		builder.Services.AddSingleton<IFirmwareService, FirmwareService>();
		builder.Services.AddSingleton<ISensorReadingService, SensorReadingService>();

#if DEBUG

		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}