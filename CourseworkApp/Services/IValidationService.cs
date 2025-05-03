using System;
using CourseworkApp.Database.Models;
using CourseworkApp.Common;
namespace CourseworkApp.Services;

public interface IValidationService
{
  ValidationResult ValidateConfig(SensorConfigurations config);
}
