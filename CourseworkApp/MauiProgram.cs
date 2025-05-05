=﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using CourseworkApp.Database.Data;
using CourseworkApp.Repositories;
using CourseworkApp.Services;
using CourseworkApp.ViewModels;
using CourseworkApp.Views;
using CourseworkApp.Services.Factory;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Controls.Xaml;
using Syncfusion.Maui.Core.Hosting;
using Microsoft.Extensions.Logging;

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

        // Load appsettings.json configuration
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        builder.Configuration.AddConfiguration(config);

        // Add EF DbContexts
        builder.Services.AddDbContextFactory<TestDbContext>();

        var connectionString = builder.Configuration
            .GetSection("ConnectionStrings:DevelopmentConnection").Value;

        builder.Services.AddDbContext<CourseDbContext>(options =>
            options.UseSqlServer(connectionString));

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Views + ViewModels
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<MainPageViewModel>();
        builder.Services.AddTransient<AdminConfig>();
        builder.Services.AddTransient<AdminConfigViewModel>();
        builder.Services.AddTransient<ConfigForm>();
        builder.Services.AddTransient<ConfigFormViewModel>();
        builder.Services.AddTransient<AdminFirmware>();
        builder.Services.Add
