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

    var result = new ValidationResult();  //Basic null check
    if (config == null)
    {
      result.Status = ValidationStatus.Failed;
      result.Errors.Add("Validation failed: Config object is null.");
      return result;
    }

    // Validate properties of SensorConfigurations
    if (string.IsNullOrWhiteSpace(config.SensorType))
    {
      result.Errors.Add("Validation Failed: SensorType is empty.");
    }

    if (string.IsNullOrWhiteSpace(config.ConfigName))
    {
      result.Errors.Add("Validation Failed: ConfigName is empty.");
    }

    // Validate the nested ConfigData object
    if (config.ConfigData == null)
    {
      result.Errors.Add("Validation Failed: ConfigData is null.");
    }

    // Validate properties of BaseSensorConfig
    // Assuming MonitorFrequencySeconds and MonitorDurationSeconds should be positive
    if (config.ConfigData.MonitorFrequencySeconds <= 0)
    {
      result.Errors.Add("Validation Failed: MonitorFrequencySeconds must be positive.");
    }

    if (config.ConfigData.MonitorDurationSeconds <= 0)
    {
      result.Errors.Add("Validation Failed: MonitorDurationSeconds must be positive.");
    }

    // Validate Latitude (between -90 and 90)
    if (config.ConfigData.LocationLatitude < -90.0 || config.ConfigData.LocationLatitude > 90.0)
    {
      result.Errors.Add("Validation Failed: LocationLatitude is out of range.");
    }

    // Validate Longitude (between -180 and 180)
    if (config.ConfigData.LocationLongitude < -180.0 || config.ConfigData.LocationLongitude > 180.0)
    {
      result.Errors.Add("Validation Failed: LocationLongitude is out of range.");
    }

    // If all checks pass
    if (result.Errors.Count == 0)
    {
      result.Status = ValidationStatus.Success;
      System.Diagnostics.Debug.WriteLine("Validation Succeeded.");
    }
    else
    {
      result.Status = ValidationStatus.Failed;
      System.Diagnostics.Debug.WriteLine($"Validation Failed with {result.Errors.Count} errors.");
    }

    return result;
  }
}
