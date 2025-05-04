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
/// Service for managing firmware configurations, including retrieval, updates, and logging operations.
/// </summary>
public class FirmwareService : IFirmwareService
{
  /// <summary>
  /// Default value for an unknown user.
  /// </summary>
  private const string UnknownUser = "Unknown";

  private readonly IDbContextFactory<TestDbContext> _dbContextFactory;
  private readonly ILoggingService _loggingService;

  /// <summary>
  /// Initializes a new instance of the <see cref="FirmwareService"/> class.
  /// </summary>
  /// <param name="dbContextFactory">Factory for creating database contexts.</param>
  /// <param name="loggingService">Service for logging warnings, errors, and information.</param>
  /// <exception cref="ArgumentNullException">Thrown if <paramref name="dbContextFactory"/> is null.</exception>
  public FirmwareService(IDbContextFactory<TestDbContext> dbContextFactory, ILoggingService loggingService)
  {
    _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
    _loggingService = loggingService;
  }

  /// <summary>
  /// Retrieves a firmware configuration by its ID.
  /// </summary>
  /// <param name="firmwareId">The ID of the firmware configuration to retrieve.</param>
  /// <returns>
  /// A task that represents the asynchronous operation. The task result contains the firmware configuration if found; otherwise, null.
  /// </returns>
  public async Task<FirmwareConfigurations?> GetFirmwareByIdAsync(int firmwareId)
  {
    if (firmwareId <= 0)
    {
      await _loggingService.LogWarningAsync($"Attempted to retrieve firmware with invalid ID: {firmwareId}.");
      return null;
    }
    try
    {
      await using var context = await _dbContextFactory.CreateDbContextAsync();
      return await context.FirmwareConfigurationsDB.FindAsync(firmwareId);
    }
    catch (Exception ex)
    {
      await _loggingService.LogErrorAsync($"Error retrieving firmware by ID: {firmwareId}.", ex);
      Debug.WriteLine($"Error getting firmware ID {firmwareId}: {ex.Message}");
      return null;
    }
  }

  /// <summary>
  /// Retrieves all firmware configurations.
  /// </summary>
  /// <returns>
  /// A task that represents the asynchronous operation. The task result contains a collection of all firmware configurations.
  /// </returns>
  public async Task<IEnumerable<FirmwareConfigurations>> GetAllFirmwareAsync()
  {
    try
    {
      await using var context = await _dbContextFactory.CreateDbContextAsync();
      return await context.FirmwareConfigurationsDB.ToListAsync();
    }
    catch (Exception ex)
    {
      await _loggingService.LogErrorAsync($"Error retrieving all firmware configurations.", ex);
      Debug.WriteLine($"Error getting all firmware configurations: {ex.Message}");
      return Enumerable.Empty<FirmwareConfigurations>();
    }
  }

  /// <summary>
  /// Updates an existing firmware configuration.
  /// </summary>
  /// <param name="firmware">The firmware configuration to update.</param>
  /// <param name="currentUser">The user performing the update.</param>
  /// <returns>
  /// A task that represents the asynchronous operation. The task result is true if the update was successful; otherwise, false.
  /// </returns>
  public async Task<bool> UpdateFirmwareAsync(FirmwareConfigurations firmware, string currentUser)
  {
    string effectiveUser = currentUser ?? UnknownUser;

    if (firmware == null)
    {
      await _loggingService.LogWarningAsync($"Attempted to update firmware with null object.", new Dictionary<string, string> { { "User", effectiveUser } });
      return false;
    }

    if (firmware.FirmwareId <= 0)
    {
      await _loggingService.LogWarningAsync($"Attempted to update firmware with invalid ID: {firmware.FirmwareId}.", new Dictionary<string, string> { { "User", effectiveUser } });
      return false;
    }
    try
    {
      await using var context = await _dbContextFactory.CreateDbContextAsync();

      var existingFirmware = await context.FirmwareConfigurationsDB
        .FirstOrDefaultAsync(f => f.FirmwareId == firmware.FirmwareId);

      if (existingFirmware == null)
      {
        await _loggingService.LogWarningAsync($"Firmware with ID {firmware.FirmwareId} not found for update.", new Dictionary<string, string> { { "User", effectiveUser } });
        Debug.WriteLine($"Firmware with ID {firmware.FirmwareId} not found for update.");
        return false;
      }

      existingFirmware.SensorType = firmware.SensorType;
      existingFirmware.FirmwareVersion = firmware.FirmwareVersion;
      existingFirmware.FirmwareData = firmware.FirmwareData;
      existingFirmware.ReleaseDate = firmware.ReleaseDate;
      existingFirmware.EndofLifeDate = firmware.EndofLifeDate;
      existingFirmware.IsActive = firmware.IsActive;

      var result = await context.SaveChangesAsync();
      bool success = result > 0;

      if (!success)
      {
        await _loggingService.LogWarningAsync($"Firmware update reported no changes saved for ID {firmware.FirmwareId}.", new Dictionary<string, string> { { "User", effectiveUser } });
      }
      else
      {
        await _loggingService.LogInfoAsync($"Firmware updated successfully (ID: {firmware.FirmwareId}).", new Dictionary<string, string> { { "User", effectiveUser }, { "FirmwareId", firmware.FirmwareId.ToString() } });
      }
      return success;
    }
    catch (Exception ex)
    {
      await _loggingService.LogErrorAsync($"Error updating firmware (ID: {firmware?.FirmwareId}).", ex, new Dictionary<string, string> { { "User", effectiveUser } });
      Debug.WriteLine($"Error updating firmware (ID: {firmware?.FirmwareId}): {ex.Message}");
      return false;
    }
  }
}