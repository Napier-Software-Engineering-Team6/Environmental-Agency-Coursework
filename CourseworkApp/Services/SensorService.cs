using CourseworkApp.Models;
using CourseworkApp.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseworkApp.Services
{
    public class SensorService
    {
        private readonly ISensorRepository _sensorRepository;

        public SensorService(ISensorRepository sensorRepository)
        {
            _sensorRepository = sensorRepository;
        }

        public Task<List<SensorModel>> GetAllSensorsAsync()
        {
            return _sensorRepository.GetAllSensorsAsync();
        }

        public Task<List<SensorModel>> GetSensorsByStatusAsync(string status)
        {
            return _sensorRepository.GetSensorsByStatusAsync(status);
        }
    }
}
