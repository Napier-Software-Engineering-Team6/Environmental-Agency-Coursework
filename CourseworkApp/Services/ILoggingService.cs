using System;
using System.Threading.Tasks;
using CourseworkApp.Enums;

namespace CourseworkApp.Services;
/// <summary>
/// Interface for logging service.
/// This interface defines methods for logging messages with different severity levels and user actions.
/// </summary>
public enum LogLevel
{
  Information,
  Warning,
  Error,
  Debug
}
/// <summary>
/// Interface for logging service.
/// This interface defines methods for logging messages with different severity levels and user actions.
/// </summary>
public interface ILoggingService
{
  Task LogAsync(LogLevel level, string message, Exception? exception = null, IDictionary<string, string?>? properties = null);

  Task LogInfoAsync(string message, IDictionary<string, string>? properties = null);
  Task LogWarningAsync(string message, IDictionary<string, string>? properties = null);
  Task LogErrorAsync(string message, Exception? exception = null, IDictionary<string, string>? properties = null);
  Task LogDebugAsync(string message, IDictionary<string, string>? properties = null);
  Task LogUserActionAsync(string action, ActionStatus status, string message, IDictionary<string, string>? properties = null);
}
