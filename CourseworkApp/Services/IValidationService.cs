using System;
using CourseworkApp.Database.Models;

namespace CourseworkApp.Services;

public interface IValidationService
{
  bool ValidateConfig(SensorConfigurations config);
}
