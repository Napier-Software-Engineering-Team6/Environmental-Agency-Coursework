using CourseworkApp.Database.Data;
using CourseworkApp.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CourseworkApp.Services
{
  public class SensorUpdateService : ISensorUpdateService
  {
    private readonly IDbContextFactory<TestDbContext> _dbContextFactory;
    private readonly IConfigurationService _configurationService;
    private readonly ILoggingService _loggingService;

    public SensorUpdateService(
      IDbContextFactory<TestDbContext> dbContextFactory,
      IConfigurationService configurationService,
      ILoggingService loggingService)
    {
      _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
      _configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
      _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
    }

    public async Task<UpdateResult> AttemptApplyConfigurationToSensorAsync(
      string sensorIdentifier, int configId, string appliedByUserId)
    {
      DateTime timestamp = DateTime.UtcNow;
      string status = "Failure";
      string details = "";
      bool validationAndLoggingSuccess = false;
      const string actionType = "Apply Configuration";

      await using var context = await _dbContextFactory.CreateDbContextAsync();


      try
      {
        var configTemplate = await _configurationService.GetConfigurationByIdAsync(configId);
        if (configTemplate == null)
        {
          throw new InvalidOperationException($"Configuration template with ID {configId} not found.");
        }

        validationAndLoggingSuccess = true;
        status = "Success";
        details = $"Logged attempt to apply config {configId} for sensor identifier '{sensorIdentifier}'. Sensor existence not validated. Sensor state NOT changed by this action.";

        await LogHistoryAsync(actionType, configId, null, timestamp, status, details, appliedByUserId);
      }
      catch (Exception ex)
      {
        validationAndLoggingSuccess = false;
        status = "Failure";
        details = $"Failed attempt to validate/log config {configId} for sensor identifier '{sensorIdentifier}': {ex.Message}";
        await _loggingService?.LogErrorAsync($"Failed during validation/logging for config {configId} on sensor identifier '{sensorIdentifier}'.", ex, new Dictionary<string, string> { { "User", appliedByUserId } });

        await LogHistoryAsync(actionType, configId, null, timestamp, status, details, appliedByUserId);
      }
      return new UpdateResult
      {
        Success = validationAndLoggingSuccess,
        Message = details
      };
    }

    private async Task LogHistoryAsync(string actionType, int? configId, int? firmwareId, DateTime timestamp, string status, string details, string performedBy)
    {
      await using var logContext = await _dbContextFactory.CreateDbContextAsync();
      var historyEntry = new SensorConfigHistory
      {
        ConfigId = configId,
        FirmwareId = firmwareId,
        ActionType = actionType,
        Status = status,
        Details = details?.Length > 1000 ? details.Substring(0, 1000) : details ?? "",
        PerformedBy = performedBy,
        Timestamp = timestamp
      };
      try
      {
        logContext.SensorConfigHistoryDB.Add(historyEntry);
        await logContext.SaveChangesAsync();
      }
      catch (Exception logEx)
      {
        string itemContext = configId.HasValue ? $"ConfigId: {configId.Value}" : $"FirmwareId: {firmwareId.Value}";
        await _loggingService?.LogErrorAsync($"CRITICAL: Failed to write to SensorConfigHistory table for {itemContext}. Action: {actionType}", logEx, new Dictionary<string, string> { { "User", performedBy } });
        Debug.WriteLine($"CRITICAL: Failed to log sensor config history: {logEx.Message}");
      }
    }
  }
}