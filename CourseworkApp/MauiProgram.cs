﻿using Microsoft.Extensions.Logging;
using CourseworkApp.Database.Data;
using CourseworkApp.ViewModels;
using CourseworkApp.Views;

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


		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<MainPageViewModel>();

#if DEBUG

		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}