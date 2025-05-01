using CourseworkApp.Database.Models;
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
        /// <param name="forceReload">If true, disables tracking to fetch updated values.</param>
        /// <returns>A task that returns a list of all sensor models.</returns>
        public virtual Task<List<SensorModel>> GetAllSensorsAsync(bool forceReload = false)
        {
            return _sensorRepository.GetAllSensorsAsync(forceReload);
        }

        /// <summary>
        /// Retrieves sensors that match a specific operational status.
        /// </summary>
        /// <param name="status">The desired status (e.g. Active, Inactive, Malfunction).</param>
        /// <returns>A task that returns a list of matching sensor models.</returns>
        public virtual Task<List<SensorModel>> GetSensorsByStatusAsync(string status)
        {
            return _sensorRepository.GetSensorsByStatusAsync(status);
        }
    }
}
