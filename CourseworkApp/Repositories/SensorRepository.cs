using CourseworkApp.Database.Data;
using CourseworkApp.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseworkApp.Repositories
{
    public class SensorRepository : ISensorRepository
    {
        private readonly TestDbContext _dbContext;

        public SensorRepository(TestDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<SensorModel>> GetAllSensorsAsync()
        {
            return await _dbContext.Sensors.ToListAsync();
        }

        public async Task<List<SensorModel>> GetSensorsByStatusAsync(string status)
        {
            return await _dbContext.Sensors
                .Where(sensor => sensor.Status == status)
                .ToListAsync();
        }
    }
}
