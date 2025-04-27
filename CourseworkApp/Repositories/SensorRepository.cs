using CourseworkApp.Database.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseworkApp.Repositories
{
    public class SensorRepository : ISensorRepository
    {
        private readonly List<SensorModel> _mockSensors;

        public SensorRepository()
        {
            // Mock some sensor data
            _mockSensors = new List<SensorModel>
            {
            new SensorModel { Id = 1, Name = "Air Sensor 1", Location = "Racoon City", Status = "Active", LastUpdated = DateTime.Now.AddMinutes(-5), Type = "Air" },
            new SensorModel { Id = 2, Name = "Water Sensor 1", Location = "Deepnest", Status = "Inactive", LastUpdated = DateTime.Now.AddMinutes(-20), Type = "Water" },
            new SensorModel { Id = 3, Name = "Weather Station 1", Location = "Zanarkand", Status = "Malfunction", LastUpdated = DateTime.Now.AddMinutes(-10), Type = "Weather" },
            new SensorModel { Id = 4, Name = "Air Sensor 2", Location = "Namek", Status = "Active", LastUpdated = DateTime.Now.AddMinutes(-2), Type = "Air" },
            new SensorModel { Id = 5, Name = "Water Sensor 2", Location = "Pallet Town", Status = "Malfunction", LastUpdated = DateTime.Now.AddMinutes(-30), Type = "Water" },
            new SensorModel { Id = 6, Name = "Air Sensor 3", Location = "Chemical Plant", Status = "Inactive", LastUpdated = DateTime.Now.AddMinutes(-15), Type = "Air" },
            new SensorModel { Id = 7, Name = "Weather Station 2", Location = "Hyrule", Status = "Active", LastUpdated = DateTime.Now.AddMinutes(-8), Type = "Weather" },
            };
        }

        public Task<List<SensorModel>> GetAllSensorsAsync()
        {
            // Simulate asynchronous database call
            return Task.FromResult(_mockSensors);
        }

        public Task<List<SensorModel>> GetSensorsByStatusAsync(string status)
        {
            var filteredSensors = _mockSensors
                .Where(s => s.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Task.FromResult(filteredSensors);
        }
    }
}
