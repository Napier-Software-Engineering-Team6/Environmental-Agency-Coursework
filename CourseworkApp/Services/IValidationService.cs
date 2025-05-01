using System;
using CourseworkApp.Database.Models;

namespace CourseworkApp.Services;

public interface IValidationService
{
  List<string> ValidateConfig(SensorConfigurations config);
}
