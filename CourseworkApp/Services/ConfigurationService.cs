using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using CourseworkApp.Database.Models;
using CourseworkApp.Database.Data;

namespace CourseworkApp.Services;

/// <summary>
/// Service for managing sensor configurations in the database.
/// </summary>
public class ConfigurationService : IConfigurationService
{
  const string UnknownUser = "Unknown";
  private readonly IDbContextFactory<TestDbContext> _dbContextFactory;
  private readonly ILoggingService _loggingService;

  /// <summary>
  /// Initializes a new instance of the <see cref="ConfigurationService"/> class.
  /// </summary>
  /// <param name="dbContextFactory">Factory for creating database contexts.</param>
  /// <param name="loggingService">Service for logging operations.</param>
  /// <exception cref="ArgumentNullException">Thrown if <paramref name="dbContextFactory"/> is null.</exception>
  public ConfigurationService(IDbContextFactory<TestDbContext> dbContextFactory, ILoggingService loggingService)
  {
    _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
    _loggingService = loggingService;
  }

  /// <summary>
  /// Retrieves a sensor configuration by its ID.
  /// </summary>
  /// <param name="configId">The ID of the configuration to retrieve.</param>
  /// <returns>The sensor configuration if found; otherwise, null.</returns>
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

  /// <summary>
  /// Retrieves all sensor configurations from the database.
  /// </summary>
  /// <returns>A collection of all sensor configurations.</returns>
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

  /// <summary>
  /// Updates an existing sensor configuration in the database.
  /// </summary>
  /// <param name="configuration">The configuration to update.</param>
  /// <param name="currentUser">The user performing the update.</param>
  /// <returns>True if the update was successful; otherwise, false.</returns>
  public async Task<bool> UpdateConfigurationAsync(SensorConfigurations configuration, string currentUser)
  {
    if (configuration == null)
    {
      if (_loggingService != null)
      {
        await _loggingService.LogWarningAsync("Attempted to update null configuration.", new Dictionary<string, string> { { "User", currentUser ?? UnknownUser } });
      }
      return false;
    }

    if (configuration.ConfigId <= 0)
    {
      if (_loggingService != null)
      {
        await _loggingService.LogWarningAsync($"Attempted to update configuration with invalid ID: {configuration.ConfigId}.", new Dictionary<string, string> { { "User", currentUser ?? UnknownUser } });
      }
      return false;
    }

    try
    {
      await using var context = await _dbContextFactory.CreateDbContextAsync();

      var existingConfig = await context.SensorConfigurationsDB
        .Include(c => c.ConfigData)
        .FirstOrDefaultAsync(c => c.ConfigId == configuration.ConfigId);

      if (existingConfig == null)
      {
        if (_loggingService != null)
        {
          await _loggingService.LogWarningAsync($"Configuration with ID {configuration.ConfigId} not found for update.", new Dictionary<string, string> { { "User", currentUser ?? UnknownUser } });
          Debug.WriteLine($"Configuration with ID {configuration.ConfigId} not found for update.");
        }
        return false;
      }

      existingConfig.ConfigName = configuration.ConfigName;
      existingConfig.IsActive = configuration.IsActive;

      if (configuration.ConfigData != null)
      {
        existingConfig.ConfigData.MonitorFrequencySeconds = configuration.ConfigData.MonitorFrequencySeconds;
        existingConfig.ConfigData.MonitorDurationSeconds = configuration.ConfigData.MonitorDurationSeconds;
        existingConfig.ConfigData.LocationLatitude = configuration.ConfigData.LocationLatitude;
        existingConfig.ConfigData.LocationLongitude = configuration.ConfigData.LocationLongitude;
      }

      var result = await context.SaveChangesAsync();
      bool success = result > 0;

      if (!success)
      {
        if (_loggingService != null)
        {
          await _loggingService.LogWarningAsync($"Configuration update reported no changes saved for ID {configuration.ConfigId}.", new Dictionary<string, string> { { "User", currentUser ?? UnknownUser } });
        }
      }
      else
      {
        if (_loggingService != null)
        {
          await _loggingService.LogInfoAsync($"Configuration updated successfully (ID: {configuration.ConfigId}).", new Dictionary<string, string> { { "User", currentUser ?? UnknownUser }, { "ConfigId", configuration.ConfigId.ToString() } });
        }
      }
      return success;
    }
    catch (Exception ex)
    {
      if (_loggingService != null)
      {
        await _loggingService.LogErrorAsync($"Error updating configuration (ID: {configuration.ConfigId}).", ex, new Dictionary<string, string> { { "User", currentUser ?? UnknownUser } });
      }
      return false;
    }
  }
}
