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

	// Configuration
	var config = new ConfigurationBuilder()
		.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
		.Build();

	builder.Configuration.AddConfiguration(config);

	var connectionString = builder.Configuration
		.GetSection("ConnectionStrings:DevelopmentConnection").Value;

	builder.Services.AddDbContextFactory<TestDbContext>();
	builder.Services.AddDbContext<CourseDbContext>(options =>
		options.UseSqlServer(connectionString));

#if DEBUG
	builder.Logging.AddDebug();
#endif

	// Register views and viewmodels
	builder.Services.AddSingleton<MainPage>();
	builder.Services.AddSingleton<MainPageViewModel>();
	builder.Services.AddSingleton<AdminConfig>();
	builder.Services.AddSingleton<AdminConfigViewModel>();
	builder.Services.AddSingleton<ConfigForm>();
	builder.Services.AddSingleton<ConfigFormViewModel>();
	builder.Services.AddSingleton<AdminFirmware>();
	builder.Services.AddSingleton<AdminFirmwareViewModel>();
	builder.Services.AddSingleton<FirmwareFormViewModel>();
	builder.Services.AddSingleton<FirmwareForm>();
	builder.Services.AddSingleton<EnvironmentalScientist>();
	builder.Services.AddSingleton<EnvironmentalScientistViewModel>();
	builder.Services.AddSingleton<SensorMap>();
	builder.Services.AddSingleton<SensorMapViewModel>();
	builder.Services.AddTransient<SensorViewModel>();
	builder.Services.AddTransient<SensorPage>();

	// Register services
	builder.Services.AddSingleton<ISensorConfigurationFactory, SensorConfigurationFactory>();
	builder.Services.AddSingleton<INavigationService, NavigationService>();
	builder.Services.AddSingleton<IValidationService, ValidationService>();
	builder.Services.AddSingleton<IConfigurationService, ConfigurationService>();
	builder.Services.AddSingleton<ILoggingService, LoggingService>();
	builder.Services.AddSingleton<ISensorHistoryService, SensorHistoryService>();
	builder.Services.AddSingleton<IFirmwareService, FirmwareService>();
	builder.Services.AddTransient<ISensorRepository, SensorRepository>();
	builder.Services.AddSingleton<IAlertService, AlertService>();
	builder.Services.AddTransient<SensorService>();

	return builder.Build();
}
