using System;
using CourseworkApp.Database.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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

  public List<string> ValidateFirmware(FirmwareConfigurations firmware)
  {
    var errors = new List<string>();

    if (firmware == null)
    {
      errors.Add("Validation failed: Firmware object is null.");
      return errors;
    }

    if (string.IsNullOrWhiteSpace(firmware.SensorType))
    {
      errors.Add("Validation Failed: SensorType is empty.");
    }


    if (string.IsNullOrWhiteSpace(firmware.FirmwareVersion))
    {
      errors.Add("Validation Failed: FirmwareVersion is empty.");
    }
    else
    {
      var versionPattern = @"^\d+\.\d+\.\d+$";
      if (!Regex.IsMatch(firmware.FirmwareVersion, versionPattern))
      {
        errors.Add("Validation Failed: FirmwareVersion format must be X.Y.Z (e.g., 1.0.0).");
      }
    }

    // Validate Dates
    // Check if dates are default/unset values
    if (firmware.ReleaseDate == DateTime.MinValue)
    {
      errors.Add("Validation Failed: Release Date must be set.");
    }

    if (firmware.EndofLifeDate == DateTime.MinValue)
    {
      errors.Add("Validation Failed: End of Life Date must be set.");
    }

    if (firmware.ReleaseDate != DateTime.MinValue && firmware.EndofLifeDate != DateTime.MinValue)
    {
      if (firmware.EndofLifeDate < firmware.ReleaseDate)
      {
        errors.Add("Validation Failed: End of Life Date cannot be earlier than Release Date.");
      }
    }

    if (errors.Count == 0)
    {
      System.Diagnostics.Debug.WriteLine("Firmware Validation Succeeded.");
    }
    else
    {
      System.Diagnostics.Debug.WriteLine($"Firmware Validation Failed with {errors.Count} errors.");
    }

    return errors;
  }
}
