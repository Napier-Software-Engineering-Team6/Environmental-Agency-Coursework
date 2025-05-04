using System.Collections.Generic;
using System.Threading.Tasks;
using CourseworkApp.Database.Models;

namespace CourseworkApp.Services;
/// <summary>
/// Interface for configuration service.
/// This interface defines methods for managing sensor configurations.
/// </summary>
public interface IConfigurationService
{
  Task<IEnumerable<SensorConfigurations>> GetAllConfigurationsAsync();
  Task<SensorConfigurations?> GetConfigurationByIdAsync(int configId);
  Task<bool> UpdateConfigurationAsync(SensorConfigurations configuration, string currentUser);
}
