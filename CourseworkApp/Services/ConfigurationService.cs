using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using CourseworkApp.Database.Models;
using CourseworkApp.Database.Data;

namespace CourseworkApp.Services;

public class ConfigurationService : IConfigurationService
{
  private readonly IDbContextFactory<TestDbContext> _dbContextFactory;
  private readonly ILoggingService _loggingService;

  public ConfigurationService(IDbContextFactory<TestDbContext> dbContextFactory, ILoggingService loggingService)
  {
    _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
    _loggingService = loggingService;
  }

  public async Task<SensorConfigurations?> GetConfigurationByIdAsync(int configId)
  {
    if (configId <= 0)
    {
      if (_loggingService != null)
      {
        await _loggingService.LogWarningAsync($"Attempted to retrieve configuration with invalid ID: {configId}.");
      }
      return null;
    }
    try
    {
      await using var context = await _dbContextFactory.CreateDbContextAsync();
      return await context.SensorConfigurationsDB.FindAsync(configId);
    }
    catch (Exception ex)
    {
      if (_loggingService != null)
      {
        await _loggingService.LogErrorAsync($"Error retrieving configuration by ID: {configId}.", ex);
      }
      Debug.WriteLine($"Error getting configuration ID {configId}: {ex.Message}");
      return null;
    }
  }
  public async Task<IEnumerable<SensorConfigurations>> GetAllConfigurationsAsync()
  {
    try
    {
      await using var context = await _dbContextFactory.CreateDbContextAsync();
      return await context.SensorConfigurationsDB.ToListAsync();
    }
    catch (Exception ex)
    {
      if (_loggingService != null)
      {
        await _loggingService.LogErrorAsync($"Error retrieving all configurations.", ex);
      }
      Debug.WriteLine($"Error getting all configurations: {ex.Message}");
      return Enumerable.Empty<SensorConfigurations>();
    }
  }

  public async Task<bool> UpdateConfigurationAsync(SensorConfigurations config, string currentUser)
  {
    if (config == null)
    {
      if (_loggingService != null)
      {
        await _loggingService.LogWarningAsync("Attempted to update null configuration.", new Dictionary<string, string> { { "User", currentUser ?? "Unknown" } });
      }
      return false;
    }

    if (config.ConfigId <= 0)
    {
      if (_loggingService != null)
      {
        await _loggingService.LogWarningAsync($"Attempted to update configuration with invalid ID: {config.ConfigId}.", new Dictionary<string, string> { { "User", currentUser ?? "Unknown" } });
      }
      return false;
    }

    try
    {

      await using var context = await _dbContextFactory.CreateDbContextAsync();


      var existingConfig = await context.SensorConfigurationsDB
        .Include(c => c.ConfigData)
        .FirstOrDefaultAsync(c => c.ConfigId == config.ConfigId);


      if (existingConfig == null)
      {
        if (_loggingService != null)
        {
          await _loggingService.LogWarningAsync($"Configuration with ID {config.ConfigId} not found for update.", new Dictionary<string, string> { { "User", currentUser ?? "Unknown" } });
          Debug.WriteLine($"Configuration with ID {config.ConfigId} not found for update.");
        }
        return false;
      }

      existingConfig.ConfigName = config.ConfigName;
      existingConfig.IsActive = config.IsActive;

      if (config.ConfigData != null)
      {
        existingConfig.ConfigData.MonitorFrequencySeconds = config.ConfigData.MonitorFrequencySeconds;
        existingConfig.ConfigData.MonitorDurationSeconds = config.ConfigData.MonitorDurationSeconds;
        existingConfig.ConfigData.LocationLatitude = config.ConfigData.LocationLatitude;
        existingConfig.ConfigData.LocationLongitude = config.ConfigData.LocationLongitude;
      }

      var result = await context.SaveChangesAsync();
      bool success = result > 0;

      if (!success)
      {
        await _loggingService?.LogWarningAsync($"Configuration update reported no changes saved for ID {config.ConfigId}.", new Dictionary<string, string> { { "User", currentUser ?? "Unknown" } });
      }
      else
      {
        await _loggingService?.LogInfoAsync($"Configuration updated successfully (ID: {config.ConfigId}).", new Dictionary<string, string> { { "User", currentUser ?? "Unknown" }, { "ConfigId", config.ConfigId.ToString() } });
      }
      return success;
    }
    catch (Exception ex)
    {
      await _loggingService?.LogErrorAsync($"Error updating configuration (ID: {config?.ConfigId}).", ex, new Dictionary<string, string> { { "User", currentUser ?? "Unknown" } });
      Debug.WriteLine($"Error updating configuration (ID: {config?.ConfigId}): {ex.Message}");
      return false;
    }
  }
}
