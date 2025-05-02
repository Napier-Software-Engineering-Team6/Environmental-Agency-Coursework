using Microsoft.Extensions.Logging;
using CourseworkApp.Database.Data;
using CourseworkApp.ViewModels;
using CourseworkApp.Views;
using CourseworkApp.Services;
using CourseworkApp.Services.Factory;

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


		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<MainPageViewModel>();
		builder.Services.AddSingleton<AdminConfig>();
		builder.Services.AddSingleton<AdminConfigViewModel>();
		builder.Services.AddSingleton<ConfigForm>();
		builder.Services.AddSingleton<ConfigFormViewModel>();
		builder.Services.AddSingleton<ConfigFormViewModel>();
		builder.Services.AddSingleton<AdminFirmware>();
		builder.Services.AddSingleton<AdminFirmwareViewModel>();
		builder.Services.AddSingleton<ISensorConfigurationFactory, SensorConfigurationFactory>();
		builder.Services.AddSingleton<INavigationService, NavigationService>();
		builder.Services.AddSingleton<IValidationService, ValidationService>();
		builder.Services.AddSingleton<IConfigurationService, ConfigurationService>();
		builder.Services.AddSingleton<ILoggingService, LoggingService>();
		builder.Services.AddSingleton<ISensorHistoryService, SensorHistoryService>();
		builder.Services.AddSingleton<IFirmwareService, FirmwareService>();

#if DEBUG

		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}