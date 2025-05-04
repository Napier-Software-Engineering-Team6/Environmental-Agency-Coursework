using System.Collections.Generic;
using System.Threading.Tasks;
using CourseworkApp.Database.Models;

namespace CourseworkApp.Services;

/// <summary>
/// Interface for managing firmware configurations.
/// Provides methods for retrieving, updating, and managing firmware data.
/// </summary>
public interface IFirmwareService
{
  /// <summary>
  /// Retrieves all firmware configurations.
  /// </summary>
  /// <returns>
  /// A task that represents the asynchronous operation. The task result contains a collection of all firmware configurations.
  /// </returns>
  Task<IEnumerable<FirmwareConfigurations>> GetAllFirmwareAsync();

  /// <summary>
  /// Retrieves a firmware configuration by its ID.
  /// </summary>
  /// <param name="firmwareId">The ID of the firmware configuration to retrieve.</param>
  /// <returns>
  /// A task that represents the asynchronous operation. The task result contains the firmware configuration if found; otherwise, null.
  /// </returns>
  Task<FirmwareConfigurations?> GetFirmwareByIdAsync(int firmwareId);

  /// <summary>
  /// Updates an existing firmware configuration.
  /// </summary>
  /// <param name="firmware">The firmware configuration to update.</param>
  /// <param name="currentUser">The user performing the update.</param>
  /// <returns>
  /// A task that represents the asynchronous operation. The task result is true if the update was successful; otherwise, false.
  /// </returns>
  Task<bool> UpdateFirmwareAsync(FirmwareConfigurations firmware, string currentUser);
}