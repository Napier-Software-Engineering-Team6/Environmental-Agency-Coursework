using Xunit;
using CourseworkApp.Services; // Ensure this using still correctly points to your LoggingService location
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseworkApp.Test.Components.Services; // Updated namespace

public class LoggingServiceTests
{
  private static LoggingService CreateLoggingService()
  {
    return new LoggingService();
  }

  [Fact]
  public async Task LogAsync_InformationLevel_CompletesSuccessfully()
  {
    var service = CreateLoggingService();
    var level = LogLevel.Information;
    var message = "This is an info message.";

    var task = service.LogAsync(level, message);
    await task;

    Assert.True(task.IsCompletedSuccessfully);
  }

  [Fact]
  public async Task LogAsync_WarningLevelWithProperties_CompletesSuccessfully()
  {
    var service = CreateLoggingService();
    var level = LogLevel.Warning;
    var message = "This is a warning message.";
    var properties = new Dictionary<string, string>
        {
            { "UserId", "123" },
            { "SessionId", "ABC" }
        };

    var task = service.LogAsync(level, message, properties: properties);
    await task;

    Assert.True(task.IsCompletedSuccessfully);
  }

  [Fact]
  public async Task LogAsync_ErrorLevelWithException_CompletesSuccessfully()
  {
    var service = CreateLoggingService();
    var level = LogLevel.Error;
    var message = "This is an error message.";
    Exception? exception = null;
    try
    {
      throw new InvalidOperationException("Something went wrong");
    }
    catch (Exception ex)
    {
      exception = ex;
    }

    var task = service.LogAsync(level, message, exception: exception);
    await task;

    Assert.NotNull(exception);
    Assert.True(task.IsCompletedSuccessfully);
  }

  [Fact]
  public async Task LogAsync_DebugLevelWithAllParameters_CompletesSuccessfully()
  {
    var service = CreateLoggingService();
    var level = LogLevel.Debug;
    var message = "This is a debug message.";
    Exception? exception = null;
    try
    {
      throw new ArgumentNullException(nameof(message));
    }
    catch (Exception ex)
    {
      exception = ex;
    }
    var properties = new Dictionary<string, string?>
        {
            { "Module", "TestModule" },
            { "Value", null }
        };

    var filteredProperties = new Dictionary<string, string>();
    foreach (var kvp in properties)
    {
      if (kvp.Value != null)
      {
        filteredProperties[kvp.Key] = kvp.Value;
      }
    }

    var task = service.LogAsync(level, message, exception, filteredProperties);
    await task;

    Assert.NotNull(exception);
    Assert.True(task.IsCompletedSuccessfully);
  }

  [Fact]
  public async Task LogAsync_NullProperties_CompletesSuccessfully()
  {
    var service = CreateLoggingService();
    var level = LogLevel.Information;
    var message = "Test with null properties";

    var task = service.LogAsync(level, message, properties: null);
    await task;

    Assert.True(task.IsCompletedSuccessfully);
  }

  [Fact]
  public async Task LogAsync_NullExceptions_CompletesSuccessfully()
  {
    var service = CreateLoggingService();
    var level = LogLevel.Error;
    var message = "Test with null exception";

    var task = service.LogAsync(level, message, exception: null);
    await task;

    Assert.True(task.IsCompletedSuccessfully);
  }

  [Fact]
  public async Task LogInfoAsync_WithMessage_CompletesSuccessfully()
  {
    var service = CreateLoggingService();
    var message = "Info helper test.";

    var task = service.LogInfoAsync(message);
    await task;

    Assert.True(task.IsCompletedSuccessfully);
  }

  [Fact]
  public async Task LogWarningAsync_WithMessageAndProperties_CompletesSuccessfully()
  {
    var service = CreateLoggingService();
    var message = "Warning helper test.";
    var properties = new Dictionary<string, string?> { { "Code", "W01" } };

    var filteredProperties = new Dictionary<string, string>();
    foreach (var kvp in properties)
    {
      if (kvp.Value != null)
      {
        filteredProperties[kvp.Key] = kvp.Value;
      }
    }

    var task = service.LogWarningAsync(message, filteredProperties);
    await task;

    Assert.True(task.IsCompletedSuccessfully);
  }

  [Fact]
  public async Task LogErrorAsync_WithMessageAndException_CompletesSuccessfully()
  {
    var service = CreateLoggingService();
    var message = "Error helper test.";
    Exception? ex = new ApplicationException("Test exception for LogErrorAsync");

    var task = service.LogErrorAsync(message, ex);
    await task;

    Assert.True(task.IsCompletedSuccessfully);
  }

  [Fact]
  public async Task LogDebugAsync_WithMessage_CompletesSuccessfully()
  {
    var service = CreateLoggingService();
    var message = "Debug helper test.";

    var task = service.LogDebugAsync(message);
    await task;

    Assert.True(task.IsCompletedSuccessfully);
  }

  [Theory]
  [InlineData("Login", "Success", "User logged in successfully.")]
  [InlineData("UpdateConfig", "Completed", "Configuration updated.")]
  [InlineData("DeleteSensor", "Failed", "Failed to delete sensor.")]
  [InlineData("DataFetch", "success", "Data fetched.")]
  [InlineData("FirmwareUpdate", "failed", "Firmware update failed.")]
  public async Task LogUserActionAsync_VariousStatuses_CompletesSuccessfully(string action, string status, string message)
  {
    var service = CreateLoggingService();
    var properties = new Dictionary<string, string?> { { "User", "Admin" } };

    var filteredProperties = new Dictionary<string, string>();
    foreach (var kvp in properties)
    {
      if (kvp.Value != null)
      {
        filteredProperties[kvp.Key] = kvp.Value;
      }
    }

    var task = service.LogUserActionAsync(action, status, message, filteredProperties);
    await task;

    Assert.True(task.IsCompletedSuccessfully);
  }
}