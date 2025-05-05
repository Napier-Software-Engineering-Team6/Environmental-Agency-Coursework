
using CourseworkApp.Database.Data;
using CourseworkApp.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CourseworkApp.Services
{
  public class SensorReadingService : ISensorReadingService
  {
    private readonly IDbContextFactory<TestDbContext> _dbContextFactory;
    private readonly ILoggingService _loggingService;

    public SensorReadingService(IDbContextFactory<TestDbContext> dbContextFactory, ILoggingService loggingService)
    {
      _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
      _loggingService = loggingService;
    }

    public async Task<IEnumerable<SensorReadings>> GetRecentReadingsWithLocationAsync(TimeSpan lookbackPeriod)
    {
      if (lookbackPeriod <= TimeSpan.Zero)
      {
        await _loggingService.LogWarningAsync($"Invalid lookback period requested: {lookbackPeriod}.", null);
        return Enumerable.Empty<SensorReadings>();
      }

      var cutoffDateTime = DateTime.UtcNow - lookbackPeriod;
      await _loggingService.LogInfoAsync($"Fetching recent readings models since {cutoffDateTime:O}.", null);

      try
      {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var readings = await context.SensorReadingsDB
            .Include(sr => sr.Config)
            .Where(sr => sr.Timestamp >= cutoffDateTime && sr.Config != null && sr.Config.ConfigData != null)
            .OrderByDescending(sr => sr.Timestamp)
            .ToListAsync();

        await _loggingService.LogInfoAsync($"Successfully retrieved {readings.Count} recent SensorReadings models.", new Dictionary<string, string> { { "LookbackMinutes", lookbackPeriod.TotalMinutes.ToString() } });
        return readings;
      }
      catch (Exception ex)
      {
        Debug.WriteLine($"Error getting recent sensor readings models: {ex.Message}");
        await _loggingService.LogErrorAsync("Error retrieving recent SensorReadings models.", ex, new Dictionary<string, string> { { "LookbackMinutes", lookbackPeriod.TotalMinutes.ToString() } });
        return [];
      }
    }
  }
}