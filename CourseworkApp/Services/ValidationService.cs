using System;
using CourseworkApp.Database.Models;
using CourseworkApp.Common;
using CourseworkApp.Enums;
using System.Collections.Generic;
using System.Linq;

namespace CourseworkApp.Services;

public class ValidationService : IValidationService
{

  public ValidationResult ValidateConfig(SensorConfigurations config)
  {

    const string ValidationFailed = "Validation Failed: ";
    const string ValidationSucceeded = "Validation Succeeded.";

    var result = new ValidationResult();  //Basic null check
    if (config == null)
    {
      result.Status = ValidationStatus.Failed;
      result.Errors.Add($"{ValidationFailed} Config object is null.");
      return result;
    }

    // Validate properties of SensorConfigurations
    if (string.IsNullOrWhiteSpace(config.SensorType))
    {
      result.Errors.Add($"{ValidationFailed} SensorType is empty.");
    }

    if (string.IsNullOrWhiteSpace(config.ConfigName))
    {
      result.Errors.Add($"{ValidationFailed} ConfigName is empty.");
    }

    // Validate the nested ConfigData object
    if (config.ConfigData == null)
    {
      result.Errors.Add($"{ValidationFailed} ConfigData is null.");
    }

    // Validate properties of BaseSensorConfig
    // Assuming MonitorFrequencySeconds and MonitorDurationSeconds should be positive
    if (config.ConfigData.MonitorFrequencySeconds <= 0)
    {
      result.Errors.Add($"{ValidationFailed} MonitorFrequencySeconds must be positive.");
    }

    if (config.ConfigData.MonitorDurationSeconds <= 0)
    {
      result.Errors.Add($"{ValidationFailed} MonitorDurationSeconds must be positive.");
    }

    // Validate Latitude (between -90 and 90)
    if (config.ConfigData.LocationLatitude < -90.0 || config.ConfigData.LocationLatitude > 90.0)
    {
      result.Errors.Add($"{ValidationFailed} LocationLatitude is out of range.");
    }

    // Validate Longitude (between -180 and 180)
    if (config.ConfigData.LocationLongitude < -180.0 || config.ConfigData.LocationLongitude > 180.0)
    {
      result.Errors.Add($"{ValidationFailed} LocationLongitude is out of range.");
    }

    // If all checks pass
    if (result.Errors.Count == 0)
    {
      result.Status = ValidationStatus.Success;
      System.Diagnostics.Debug.WriteLine($"{ValidationSucceeded}.");
    }
    else
    {
      result.Status = ValidationStatus.Failed;
      System.Diagnostics.Debug.WriteLine($"Validation Failed with {result.Errors.Count} errors.");
    }

    return result;
  }
}
