using CourseworkApp.Database.Models;
using CourseworkApp.Models.Enums;
using CourseworkApp.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseworkApp.Services
{
    /// <summary>
    /// Handles sensor-related business logic. Acts as a bridge between the UI (ViewModels) and data access layer (Repositories).
    /// </summary>
    public class SensorService
    {
        private readonly ISensorRepository _sensorRepository;

        /// <summary>
        /// Constructor that injects the sensor repository dependency.
        /// </summary>
        /// <param name="sensorRepository">Instance of ISensorRepository for data access.</param>
        public SensorService(ISensorRepository sensorRepository)
        {
            _sensorRepository = sensorRepository;
        }

        /// <summary>
        /// Retrieves all sensors, optionally using AsNoTracking to force a fresh DB query.
        /// </summary>
        public virtual Task<List<SensorModel>> GetAllSensorsAsync(bool forceReload = false)
        {
            return _sensorRepository.GetAllSensorsAsync(forceReload);
        }

        /// <summary>
        /// Retrieves sensors that match a specific operational status.
        /// </summary>
        public virtual Task<List<SensorModel>> GetSensorsByStatusAsync(SensorStatus status)
        {
            return _sensorRepository.GetSensorsByStatusAsync(status);
        }
    }
}
