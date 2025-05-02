// 
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using CourseworkApp.Database.Models;
using CourseworkApp.Database.Data;

namespace CourseworkApp.Services;

public class FirmwareService : IFirmwareService
{
  private readonly IDbContextFactory<TestDbContext> _dbContextFactory;
  private readonly ILoggingService _loggingService;

  public FirmwareService(IDbContextFactory<TestDbContext> dbContextFactory, ILoggingService loggingService)
  {
    _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
    _loggingService = loggingService;
  }

  public async Task<FirmwareConfigurations?> GetFirmwareByIdAsync(int firmwareId)
  {
    if (firmwareId <= 0)
    {
      if (_loggingService != null)
      {
        await _loggingService.LogWarningAsync($"Attempted to retrieve firmware with invalid ID: {firmwareId}.");
      }
      return null;
    }
    try
    {
      await using var context = await _dbContextFactory.CreateDbContextAsync();
      return await context.FirmwareConfigurationsDB.FindAsync(firmwareId);
    }
    catch (Exception ex)
    {
      if (_loggingService != null)
      {
        await _loggingService.LogErrorAsync($"Error retrieving firmware by ID: {firmwareId}.", ex);
      }
      Debug.WriteLine($"Error getting firmware ID {firmwareId}: {ex.Message}");
      return null;
    }
  }
  public async Task<IEnumerable<FirmwareConfigurations>> GetAllFirmwareAsync()
  {
    try
    {
      await using var context = await _dbContextFactory.CreateDbContextAsync();
      return await context.FirmwareConfigurationsDB.ToListAsync();
    }
    catch (Exception ex)
    {
      if (_loggingService != null)
      {
        await _loggingService.LogErrorAsync($"Error retrieving all firmware configurations.", ex);
      }
      Debug.WriteLine($"Error getting all firmware configurations: {ex.Message}");
      return Enumerable.Empty<FirmwareConfigurations>();
    }
  }
  public async Task<bool> UpdateFirmwareAsync(FirmwareConfigurations firmware, string currentUser)
  {
    if (firmware == null)
    {
      if (_loggingService != null)
      {
        await _loggingService.LogWarningAsync($"Attempted to update firmware with null object.", new Dictionary<string, string> { { "User", currentUser ?? "Unknown" } });
      }
      return false;
    }

    if (firmware.FirmwareId <= 0)
    {
      if (_loggingService != null)
      {
        await _loggingService?.LogWarningAsync($"Attempted to update firmware with invalid ID: {firmware.FirmwareId}.", new Dictionary<string, string> { { "User", currentUser ?? "Unknown" } });
      }
      return false;
    }
    try
    {
      await using var context = await _dbContextFactory.CreateDbContextAsync();

      var existingFirmware = await context.FirmwareConfigurationsDB
        .FirstOrDefaultAsync(f => f.FirmwareId == firmware.FirmwareId);

      if (existingFirmware == null)
      {
        if (_loggingService != null) await _loggingService?.LogWarningAsync($"Firmware with ID {firmware.FirmwareId} not found for update.", new Dictionary<string, string> { { "User", currentUser ?? "Unknown" } });
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
        await _loggingService?.LogWarningAsync($"Firmware update reported no changes saved for ID {firmware.FirmwareId}.", new Dictionary<string, string> { { "User", currentUser ?? "Unknown" } });
      }
      else
      {
        await _loggingService?.LogInfoAsync($"Firmware updated successfully (ID: {firmware.FirmwareId}).", new Dictionary<string, string> { { "User", currentUser ?? "Unknown" }, { "FirmwareId", firmware.FirmwareId.ToString() } });
      }
      return success;
    }
    catch (Exception ex)
    {
      await _loggingService?.LogErrorAsync($"Error updating firmware (ID: {firmware?.FirmwareId}).", ex, new Dictionary<string, string> { { "User", currentUser ?? "Unknown" } });
      Debug.WriteLine($"Error updating firmware (ID: {firmware?.FirmwareId}): {ex.Message}");
      return false;
    }
  }
}