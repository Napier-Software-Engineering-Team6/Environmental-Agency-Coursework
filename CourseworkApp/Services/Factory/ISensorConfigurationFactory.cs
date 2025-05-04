using System;
using CourseworkApp.Database.Models;

namespace CourseworkApp.Services.Factory;
// <summary>
// Factory interface for creating default sensor configurations.
public interface ISensorConfigurationFactory
{
  SensorConfigurations CreateDefault();
}
