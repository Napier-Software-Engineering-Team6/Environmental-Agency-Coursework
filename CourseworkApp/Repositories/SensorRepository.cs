using CourseworkApp.Database.Data;
using CourseworkApp.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseworkApp.Repositories
{
    public class SensorRepository : ISensorRepository
    {
        private readonly TestDbContext _context;

        public SensorRepository()
        {
            _context = new TestDbContext();
        }

        public async Task<List<SensorModel>> GetAllSensorsAsync()
        {
            return await _context.Sensors.ToListAsync();
        }

        public async Task<List<SensorModel>> GetSensorsByStatusAsync(string status)
        {
            return await _context.Sensors
                .Where(s => s.Status == status)
                .ToListAsync();
        }
    }
}
