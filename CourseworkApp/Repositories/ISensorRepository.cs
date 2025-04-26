using CourseworkApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseworkApp.Repositories
{
    public interface ISensorRepository
    {
        Task<List<SensorModel>> GetAllSensorsAsync();

        Task<List<SensorModel>> GetSensorsByStatusAsync(string status);
    }
}
