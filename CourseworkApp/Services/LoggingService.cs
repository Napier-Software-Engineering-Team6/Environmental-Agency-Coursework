using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using CourseworkApp.Enums;

namespace CourseworkApp.Services;

public class LoggingService : ILoggingService
{
  public Task LogAsync(LogLevel level, string message, Exception? exception = null, IDictionary<string, string>? properties = null)
  {
    var logMessage = new StringBuilder();
    logMessage.Append($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] [{level.ToString().ToUpper()}] {message}");

    if (properties != null)
    {
      logMessage.Append(" Properties: ");
      foreach (var prop in properties)
      {
        logMessage.Append($"{prop.Key}={prop.Value}; ");
      }
      logMessage.Append(" }");
    }

    if (exception != null)
    {
      logMessage.AppendLine();
      logMessage.Append($"Exception: {exception.GetType().Name} - {exception.Message}");
      logMessage.AppendLine(exception.StackTrace);
    }

    Debug.WriteLine(logMessage.ToString());
    return Task.CompletedTask;
  }

  public Task LogInfoAsync(string message, IDictionary<string, string>? properties = null)
  {
    return LogAsync(LogLevel.Information, message, properties: properties);
  }

  public Task LogWarningAsync(string message, IDictionary<string, string>? properties = null)
  {
    return LogAsync(LogLevel.Warning, message, properties: properties);
  }

  public Task LogErrorAsync(string message, Exception? exception = null, IDictionary<string, string>? properties = null)
  {
    return LogAsync(LogLevel.Error, message, exception, properties);
  }

  public Task LogDebugAsync(string message, IDictionary<string, string>? properties = null)
  {
    return LogAsync(LogLevel.Debug, message, properties: properties);
  }

  public Task LogUserActionAsync(string action, ActionStatus status, string message, IDictionary<string, string>? properties = null)
  {
    var combinedMessage = $"User Action:, Action='{action}', Status='{status}', - {message}";

    var level = status == ActionStatus.Failed ? LogLevel.Error : LogLevel.Information;

    return LogAsync(level, combinedMessage, properties: properties);
  }
}
