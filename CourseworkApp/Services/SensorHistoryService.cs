
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CourseworkApp.Database.Data;
using CourseworkApp.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseworkApp.Services
{

  public class SensorHistoryService : ISensorHistoryService
  {
    private readonly IDbContextFactory<TestDbContext> _dbContextFactory;
    private readonly ILoggingService? _loggingService;

    public SensorHistoryService(IDbContextFactory<TestDbContext> dbContextFactory, ILoggingService? loggingService)

    {
      _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
      _loggingService = loggingService;
    }


    public async Task LogActionAsync(int? configId, int? firmwareId, string actionType, string status, string details, string performedBy)
    {
      if (string.IsNullOrWhiteSpace(actionType) || string.IsNullOrWhiteSpace(status) || string.IsNullOrWhiteSpace(performedBy))
      {
        await _loggingService?.LogWarningAsync("Attempted to log history action with missing required parameters.",
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
        await _loggingService?.LogErrorAsync(
            $"CRITICAL: Failed to write action to SensorConfigHistory. ActionType: {actionType}, Status: {status}, ConfigId: {configId}, FirmwareId: {firmwareId}",
            logEx,
            new Dictionary<string, string> { { "User", performedBy } });
      }
    }
  }
}