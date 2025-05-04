using System;
using CourseworkApp.Database.Models;
using CourseworkApp.Common;
using CourseworkApp.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CourseworkApp.Services;
/// <summary>
/// Service for validating sensor configurations. 
/// </summary>
public class ValidationService : IValidationService
{
  /// <summary>
  /// Validates the given sensor configuration.
  /// </summary>
  /// <param name="config"></param>
  /// <returns></returns>
  public ValidationResult ValidateConfig(SensorConfigurations config)
  {

    const string ValidationFailed = "Validation Failed:";
    const string ValidationSucceeded = "Validation Succeeded.";

    var result = new ValidationResult();  //Basic null check
    if (config == null)
    {
      result.Status = ValidationStatus.Failed;
      result.Errors.Add($"{ValidationFailed} Config object is null.");
      return result;
    }
    else
    {

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
        result.Status = ValidationStatus.Failed;
        return result;
      }
      else
      {
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
