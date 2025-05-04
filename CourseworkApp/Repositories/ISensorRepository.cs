using CourseworkApp.Database.Models;
using CourseworkApp.Models.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseworkApp.Repositories
{
    public interface ISensorRepository
    {
        Task<List<SensorModel>> GetAllSensorsAsync(bool forceReload = false);
        Task<List<SensorModel>> GetSensorsByStatusAsync(SensorStatus status);
    }
}