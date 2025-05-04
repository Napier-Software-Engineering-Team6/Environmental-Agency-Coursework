using System;
using CourseworkApp.Database.Models;
using CourseworkApp.Common;
namespace CourseworkApp.Services;
/// <summary>
/// Interface for validation service.
/// This interface defines methods for validating sensor configurations.
/// </summary>
public interface IValidationService
{
  List<string> ValidateConfig(SensorConfigurations config);
  ValidationResult ValidateConfig(SensorConfigurations config);
}
