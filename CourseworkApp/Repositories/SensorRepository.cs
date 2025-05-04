using CourseworkApp.Database.Models;
using CourseworkApp.Models.Enums;
using CourseworkApp.Database.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseworkApp.Repositories
{
    /// <summary>
    /// Repository for accessing sensor data from the database.
    /// </summary>
    public class SensorRepository : ISensorRepository
    {
        private readonly CourseDbContext dbContext;

        /// <summary>
        /// Constructs the repository with the application's database context.
        /// </summary>
        /// <param name="dbContext">Injected database context.</param>
        public SensorRepository(CourseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves all sensors from the database.
        /// </summary>
        public async Task<List<SensorModel>> GetAllSensorsAsync(bool forceReload = false)
        {
            if (forceReload)
            {
                return await dbContext.Sensors
                    .AsNoTracking()
                    .ToListAsync();
            }

            return await dbContext.Sensors.ToListAsync();
        }

        /// <summary>
        /// Retrieves sensors by a specific status.
        /// </summary>
        /// <param name="status">The sensor status to filter by.</param>
        public async Task<List<SensorModel>> GetSensorsByStatusAsync(SensorStatus status)
        {
            return await dbContext.Sensors
                .Where(sensor => sensor.Status == status)
                .ToListAsync();
        }
    }
}