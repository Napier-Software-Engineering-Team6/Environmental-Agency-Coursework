using CourseworkApp.Database.Models;
using CourseworkApp.Models.Enums;
using CourseworkApp.Repositories;
using CourseworkApp.Services;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CourseworkApp.Test.Services
{
    /// <summary>
    /// Unit tests for the SensorService class.
    /// Ensures correct behavior of business logic that retrieves sensor data.
    /// </summary>
    public class SensorServiceTests
    {
        private readonly Mock<ISensorRepository> _mockSensorRepository;
        private readonly SensorService _sensorService;

        /// <summary>
        /// Sets up the SensorService with a mocked repository for unit testing.
        /// </summary>
        public SensorServiceTests()
        {
            _mockSensorRepository = new Mock<ISensorRepository>();
            _sensorService = new SensorService(_mockSensorRepository.Object);
        }

        /// <summary>
        /// Verifies that GetSensorsByStatusAsync correctly filters and returns sensors
        /// matching the specified status.
        /// </summary>
        [Fact]
        public async Task GetSensorsByStatusAsync_ShouldReturnOnlyMatchingSensors()
        {
            // Arrange
            var status = SensorStatus.Active;

            var expectedSensors = new List<SensorModel>
            {
                new SensorModel { Id = 1, Name = "Sensor A", Status = SensorStatus.Active },
                new SensorModel { Id = 2, Name = "Sensor B", Status = SensorStatus.Active }
            };

            _mockSensorRepository
                .Setup(repo => repo.GetSensorsByStatusAsync(SensorStatus.Active))
                .ReturnsAsync(expectedSensors);

            // Act
            var actualSensors = await _sensorService.GetSensorsByStatusAsync(SensorStatus.Active);

            // Assert
            Assert.Equal(expectedSensors.Count, actualSensors.Count);
            Assert.All(actualSensors, s => Assert.Equal(status, s.Status));

            // Verify correct repository call
            _mockSensorRepository.Verify(repo => repo.GetSensorsByStatusAsync(SensorStatus.Active), Times.Once);
        }
    }
}