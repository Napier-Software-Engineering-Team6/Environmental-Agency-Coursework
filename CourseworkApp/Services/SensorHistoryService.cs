using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CourseworkApp.Database.Data;
using CourseworkApp.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseworkApp.Services
{
  /// <summary>
  /// Service for managing and logging sensor configuration history.
  /// </summary>
  public class SensorHistoryService : ISensorHistoryService
  {
    private readonly IDbContextFactory<TestDbContext> _dbContextFactory;
    private readonly ILoggingService? _loggingService;

    /// <summary>
    /// Initializes a new instance of the <see cref="SensorHistoryService"/> class.
    /// </summary>
    /// <param name="dbContextFactory">Factory for creating database contexts.</param>
    /// <param name="loggingService">Optional logging service for logging warnings and errors.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="dbContextFactory"/> is null.</exception>
    public SensorHistoryService(IDbContextFactory<TestDbContext> dbContextFactory, ILoggingService? loggingService)
    {
      _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
      _loggingService = loggingService;
    }

    /// <summary>
    /// Logs an action to the sensor configuration history.
    /// </summary>
    /// <param name="configId">The ID of the sensor configuration (nullable).</param>
    /// <param name="firmwareId">The ID of the firmware (nullable).</param>
    /// <param name="actionType">The type of action performed.</param>
    /// <param name="status">The status of the action.</param>
    /// <param name="details">Additional details about the action.</param>
    /// <param name="performedBy">The user who performed the action.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <remarks>
    /// If required parameters are missing, a warning is logged and the method returns without saving to the database.
    /// If an error occurs while saving to the database, it is logged as a critical error.
    /// </remarks>
    public async Task LogActionAsync(int? configId, int? firmwareId, string actionType, string status, string details, string performedBy)
    {
      if (string.IsNullOrWhiteSpace(actionType) || string.IsNullOrWhiteSpace(status) || string.IsNullOrWhiteSpace(performedBy))
      {
        if (_loggingService != null)
        {
          await _loggingService.LogWarningAsync("Attempted to log history action with missing required parameters.",
            new Dictionary<string, string> { { "ActionType", actionType ?? "NULL" }, { "Status", status ?? "NULL" }, { "PerformedBy", performedBy ?? "NULL" } });
          return;
        }

        var historyEntry = new SensorConfigHistory
        {
          ConfigId = configId,
          FirmwareId = firmwareId,
          ActionType = actionType,
          Status = status,
          Details = details,
          PerformedBy = performedBy,
          Timestamp = DateTime.UtcNow
        };

        try
        {
          await using var logContext = await _dbContextFactory.CreateDbContextAsync();
          logContext.SensorConfigHistoryDB.Add(historyEntry);
          await logContext.SaveChangesAsync();
        }
        catch (Exception logEx)
        {
          if (_loggingService != null)
          {
            await _loggingService.LogErrorAsync(
                $"CRITICAL: Failed to write action to SensorConfigHistory. ActionType: {actionType}, Status: {status}, ConfigId: {configId}, FirmwareId: {firmwareId}",
                logEx,
                new Dictionary<string, string> { { "User", performedBy } });
          }
        }
      }
    }
  }
}