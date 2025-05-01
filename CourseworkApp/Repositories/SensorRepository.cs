using CourseworkApp.Database.Data;
using CourseworkApp.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseworkApp.Repositories
{
    /// <summary>
    /// Provides access to sensor data from the database.
    /// Implements methods for retrieving all sensors or filtering by status.
    /// </summary>
    public class SensorRepository : ISensorRepository
    {
        private readonly CourseDbContext _dbContext;

        /// <summary>
        /// Constructor that receives the CourseDbContext via dependency injection.
        /// </summary>
        /// <param name="dbContext">Database context for accessing sensor data.</param>
        public SensorRepository(CourseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves all sensors from the database.
        /// Optionally disables tracking for better performance if reload is required.
        /// </summary>
        /// <param name="forceReload">If true, disables EF tracking to get fresh data.</param>
        /// <returns>A list of SensorModel objects.</returns>
        public async Task<List<SensorModel>> GetAllSensorsAsync(bool forceReload = false)
        {
            var query = _dbContext.Sensors.AsQueryable();

            if (forceReload)
                query = query.AsNoTracking();

            return await query.OrderBy(s => s.Id).ToListAsync();
        }

        /// <summary>
        /// Retrieves sensors from the database that match a specific status.
        /// </summary>
        /// <param name="status">Sensor status to filter by (e.g., Active, Inactive).</param>
        /// <returns>A filtered list of SensorModel objects.</returns>
        public async Task<List<SensorModel>> GetSensorsByStatusAsync(string status)
        {
            return await _dbContext.Sensors
                .Where(sensor => sensor.Status == status)
                .ToListAsync();
        }
    }
}
