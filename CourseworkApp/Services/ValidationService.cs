using System;
using CourseworkApp.Database.Models;

namespace CourseworkApp.Services;

public class ValidationService : IValidationService
{
  public bool ValidateConfig(SensorConfigurations config)
  {
    //Basic null check
    if (config == null)
    {
      System.Diagnostics.Debug.WriteLine("Validation failed: Config object is null.");
      return false;
    }

    // Validate properties of SensorConfigurations
    if (string.IsNullOrWhiteSpace(config.SensorType))
    {
      System.Diagnostics.Debug.WriteLine("Validation Failed: SensorType is empty.");
      return false;
    }

    if (string.IsNullOrWhiteSpace(config.ConfigName))
    {
      System.Diagnostics.Debug.WriteLine("Validation Failed: ConfigName is empty.");
      return false;
    }

    // Validate the nested ConfigData object
    if (config.ConfigData == null)
    {
      System.Diagnostics.Debug.WriteLine("Validation Failed: ConfigData is null.");
      return false;
    }

    // Validate properties of BaseSensorConfig
    // Assuming MonitorFrequencySeconds and MonitorDurationSeconds should be positive
    if (config.ConfigData.MonitorFrequencySeconds <= 0)
    {
      System.Diagnostics.Debug.WriteLine("Validation Failed: MonitorFrequencySeconds must be positive.");
      return false;
    }

    if (config.ConfigData.MonitorDurationSeconds <= 0)
    {
      System.Diagnostics.Debug.WriteLine("Validation Failed: MonitorDurationSeconds must be positive.");
      return false;
    }

    // Validate Latitude (between -90 and 90)
    if (config.ConfigData.LocationLatitude < -90.0 || config.ConfigData.LocationLatitude > 90.0)
    {
      System.Diagnostics.Debug.WriteLine("Validation Failed: LocationLatitude is out of range.");
      return false;
    }

    // Validate Longitude (between -180 and 180)
    if (config.ConfigData.LocationLongitude < -180.0 || config.ConfigData.LocationLongitude > 180.0)
    {
      System.Diagnostics.Debug.WriteLine("Validation Failed: LocationLongitude is out of range.");
      return false;
    }

    // If all checks pass
    System.Diagnostics.Debug.WriteLine("Validation Succeeded.");
    return true;
  }
}
