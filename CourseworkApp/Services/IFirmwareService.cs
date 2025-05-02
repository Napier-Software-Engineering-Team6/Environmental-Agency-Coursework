using System.Collections.Generic;
using System.Threading.Tasks;
using CourseworkApp.Database.Models;

namespace CourseworkApp.Services;

public interface IFirmwareService
{
  Task<IEnumerable<FirmwareConfigurations>> GetAllFirmwareAsync();


  Task<FirmwareConfigurations?> GetFirmwareByIdAsync(int firmwareId);

  Task<bool> UpdateFirmwareAsync(FirmwareConfigurations firmware, string currentUser);
}