using System;
using CourseworkApp.Database.Models;
using System.Collections.Generic;
using System.Linq;

namespace CourseworkApp.Services;

public class ValidationService : IValidationService
{

  public List<string> ValidateConfig(SensorConfigurations config)
  {

    var errors = new List<string>();
    //Basic null check
    if (config == null)
    {
      errors.Add("Validation failed: Config object is null.");
      return errors;
    }

    // Validate properties of SensorConfigurations
    if (string.IsNullOrWhiteSpace(config.SensorType))
    {
      errors.Add("Validation Failed: SensorType is empty.");
    }

    if (string.IsNullOrWhiteSpace(config.ConfigName))
    {
      errors.Add("Validation Failed: ConfigName is empty.");
    }

    // Validate the nested ConfigData object
    if (config.ConfigData == null)
    {
      errors.Add("Validation Failed: ConfigData is null.");
    }

    // Validate properties of BaseSensorConfig
    // Assuming MonitorFrequencySeconds and MonitorDurationSeconds should be positive
    if (config.ConfigData.MonitorFrequencySeconds <= 0)
    {
      errors.Add("Validation Failed: MonitorFrequencySeconds must be positive.");
    }

    if (config.ConfigData.MonitorDurationSeconds <= 0)
    {
      errors.Add("Validation Failed: MonitorDurationSeconds must be positive.");
    }

    // Validate Latitude (between -90 and 90)
    if (config.ConfigData.LocationLatitude < -90.0 || config.ConfigData.LocationLatitude > 90.0)
    {
      errors.Add("Validation Failed: LocationLatitude is out of range.");
    }

    // Validate Longitude (between -180 and 180)
    if (config.ConfigData.LocationLongitude < -180.0 || config.ConfigData.LocationLongitude > 180.0)
    {
      errors.Add("Validation Failed: LocationLongitude is out of range.");
    }

    // If all checks pass
    if (errors.Count == 0)
    {
      System.Diagnostics.Debug.WriteLine("Validation Succeeded.");
    }
    else
    {
      System.Diagnostics.Debug.WriteLine($"Validation Failed with {errors.Count} errors.");
    }

    return errors;
  }
}
