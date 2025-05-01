using System;
using CourseworkApp.Database.Models;

namespace CourseworkApp.Services.Factory;

public interface ISensorConfigurationFactory
{
  SensorConfigurations CreateDefault();
}
