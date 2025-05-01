using System.Collections.Generic;
using System.Threading.Tasks;
using CourseworkApp.Database.Models;

namespace CourseworkApp.Services;

public interface IConfigurationService
{
  Task<IEnumerable<SensorConfigurations>> GetAllConfigurationsAsync();
  Task<SensorConfigurations?> GetConfigurationByIdAsync(int configId);
  Task<bool> UpdateConfigurationAsync(SensorConfigurations configuration, string currentUser);
}
