using CourseworkApp.Models;
using CourseworkApp.Repositories;
using CourseworkApp.Services;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CourseworkApp.Tests.Services
{
    public class SensorServiceTests
    {
        private readonly Mock<ISensorRepository> _mockRepository;
        private readonly SensorService _sensorService;

        public SensorServiceTests()
        {
            _mockRepository = new Mock<ISensorRepository>();
            _sensorService = new SensorService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllSensorsAsync_ReturnsAllSensors()
        {
            // Arrange
            var expectedSensors = new List<SensorModel>
            {
                new SensorModel { Id = 1, Name = "Sensor 1", Status = "Active" },
                new SensorModel { Id = 2, Name = "Sensor 2", Status = "Inactive" }
            };

            _mockRepository.Setup(repo => repo.GetAllSensorsAsync())
                           .ReturnsAsync(expectedSensors);

            // Act
            var result = await _sensorService.GetAllSensorsAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Sensor 1", result[0].Name);
            Assert.Equal("Sensor 2", result[1].Name);
        }

        [Fact]
        public async Task GetSensorsByStatusAsync_ReturnsFilteredSensors()
        {
            // Arrange
            var sensors = new List<SensorModel>
            {
                new SensorModel { Id = 1, Name = "Sensor 1", Status = "Active" },
                new SensorModel { Id = 2, Name = "Sensor 2", Status = "Inactive" }
            };

            _mockRepository.Setup(repo => repo.GetSensorsByStatusAsync("Active"))
                           .ReturnsAsync(new List<SensorModel> { sensors[0] });

            // Act
            var result = await _sensorService.GetSensorsByStatusAsync("Active");

            // Assert
            Assert.Single(result);
            Assert.Equal("Active", result[0].Status);
        }
    }
}
