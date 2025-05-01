using System;
using CourseworkApp.Database.Models;


namespace CourseworkApp.Services.Factory;

public class SensorConfigurationFactory : ISensorConfigurationFactory
{
  public SensorConfigurations CreateDefault()
  {
    return new SensorConfigurations
    {
      ConfigId = 0,
      SensorType = "Default",
      ConfigName = "New Sensor Configuration",
      ConfigData = new BaseSensorConfig
      {
        MonitorFrequencySeconds = 60,
        MonitorDurationSeconds = 300,
        LocationLatitude = 0.0,
        LocationLongitude = 0.0
      },
      CreatedAt = DateTime.UtcNow,
      IsActive = true
    };
  }
}
