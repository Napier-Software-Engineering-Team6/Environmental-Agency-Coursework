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

        public async Task<List<SensorModel>> GetAllSensorsAsync(bool forceReload = false)
        {
            var query = _dbContext.Sensors.AsQueryable();

            if (forceReload)
                query = query.AsNoTracking();

            return await query.OrderBy(s => s.Id).ToListAsync();
        }

        public async Task<List<SensorModel>> GetSensorsByStatusAsync(string status)
        {
            return await _dbContext.Sensors
                .Where(sensor => sensor.Status == status)
                .ToListAsync();
        }
    }
}
